using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bot.Interfaces.Discord.Handlers.CommandHandlers;
using Bot.Interfaces.Discord.Services;
using Bot.Logger.Interfaces;
using Bot.Persistence.Domain;
using Bot.Persistence.UnitOfWorks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Bot.Discord.Handlers.CommandHandlers
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordShardedClient _client;
        private readonly CommandService _commandService;
        private readonly ILogger _logger;
        private readonly IPrefixService _prefixService;
        private readonly ICommandErrorHandler _commandErrorHandler;
        private readonly ICommandInputErrorHandler _commandInputErrorHandler;
        private readonly ISpamFilter _spamFilter;
        private IServiceProvider _services;


        /// <summary>
        /// Creates a new <see cref="CommandHandler"/>.
        /// </summary>
        /// <param name="client">The <see cref="DiscordShardedClient"/> that will be used to receive all the messages.</param>
        /// <param name="commandService">The <see cref="CommandService"/> that will be used to execute the commands.</param>
        /// <param name="logger">The <see cref="ILogger"/> that will be used to log all the messages.</param>
        /// <param name="prefixService">The <see cref="IPrefixService"/> that will be used.</param>
        /// <param name="commandErrorHandler">The <see cref="ICommandErrorHandler"/> that will be used to handle command errors.</param>
        /// <param name="commandInputErrorHandler">The <see cref="ICommandInputErrorHandler"/> that will be used when the input for a command is wrong.</param>
        /// <param name="spamFilter">The <see cref="ISpamFilter"/> that will be used to filter out users that are spamming commands.</param>
        public CommandHandler(DiscordShardedClient client, CommandService commandService, ILogger logger, IPrefixService prefixService,
                              ICommandErrorHandler commandErrorHandler, ICommandInputErrorHandler commandInputErrorHandler, ISpamFilter spamFilter)
        {
            _client = client;
            _commandService = commandService;
            _logger = logger;
            _prefixService = prefixService;
            _commandErrorHandler = commandErrorHandler;
            _commandInputErrorHandler = commandInputErrorHandler;
            _spamFilter = spamFilter;
        }

        /// <inheritdoc />
        public async Task InitializeAsync()
        {
            // Add all objects that are needed for the commands.
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddScoped<ILogger, Logger.Logger>()
                .BuildServiceProvider();

            // Add all commands and services to the command service.
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services).ConfigureAwait(false);

            _commandService.Log += LogCommandServiceEvent;
            _client.MessageReceived += HandleCommandEvent;
        }


        /// <summary>
        /// Handles the given log message.
        /// </summary>
        /// <param name="logMessage">The log message that will be logged.</param>
        private Task LogCommandServiceEvent(LogMessage logMessage)
        {
            Task.Run(async () => await CommandServiceLogAsync(logMessage).ConfigureAwait(false));
            return Task.CompletedTask;
        }


        /// <summary>
        /// Handles the given message.
        /// </summary>
        /// <param name="message">The socket message.</param>
        private Task HandleCommandEvent(SocketMessage message)
        {
            if (!(message is SocketUserMessage msg)) return Task.CompletedTask;
            Task.Run(async () => await CheckForPrefixAsync(msg).ConfigureAwait(false));
            return Task.CompletedTask;
        }


        /// <summary>
        /// Checks if the message contains a prefix.
        /// </summary>
        /// <param name="msg">The socket user message.</param>
        private async Task CheckForPrefixAsync(SocketUserMessage msg)
        {

            // If the user is a bot, return.
            if (msg.Author.IsBot) return;

            // Create the shared command context.
            var context = new ShardedCommandContext(_client, msg);
            var argPos = 0;

            // Check if the message has a valid default command prefix.
            if (context.Message.HasStringPrefix(Constants.Prefix, ref argPos, StringComparison.CurrentCultureIgnoreCase) ||
                context.Message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                await HandleCommandAsync(context, argPos).ConfigureAwait(false);
            }

            // Check if the message has a valid custom command prefix.
            var customPrefix = _prefixService.GetPrefix(context.Guild.Id);
            if (customPrefix == null) return;
            if (context.Message.HasStringPrefix(customPrefix, ref argPos, StringComparison.CurrentCultureIgnoreCase)) await HandleCommandAsync(context, argPos).ConfigureAwait(false);
        }


        /// <summary>
        /// Checks if the message contains a prefix.
        /// </summary>
        /// <param name="context">The sharded command context that was created in <see cref="CheckForPrefixAsync"/>.</param>
        /// <param name="argPos">The argPos to be used.</param>
        private async Task HandleCommandAsync(ShardedCommandContext context, int argPos)
        {
            try
            {

                // Start a stopwatch to log the runtime. 
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Searching for the command that should be executed.
                var searchResult = _commandService.Search(context, argPos);

                // If no command were found, return.
                if (searchResult.Commands == null || searchResult.Commands.Count == 0)
                {
                    stopwatch.Stop();
                    return;
                }

                // If the user is spamming commands, return.
                if (!await _spamFilter.FilterAsync(context).ConfigureAwait(false))
                {
                    stopwatch.Stop();
                    await SaveRequestDataAsync(stopwatch, context, searchResult, false, "Blocked by spam filter").ConfigureAwait(false);
                    return;
                }

                // Execute the command.
                var result = await _commandService.ExecuteAsync(context, argPos, _services).ConfigureAwait(false);

                // If result of the command is is un success full, send a embedded error message.
                // Warning: This will only be false if the input is wrong and not when a error is occuring while the command is running.
                if (!result.IsSuccess)
                {
                    _logger.Log("Error", result.ErrorReason, context.Message.Content, context.User.Username, context.Guild.Name, context.Guild.Id);
                    var embed = await _commandInputErrorHandler.HandleErrorsAsync(result, context).ConfigureAwait(false);
                    if (embed != null) await context.Channel.SendMessageAsync("", false, embed.Build()).ConfigureAwait(false);
                }

                stopwatch.Stop();
                await SaveRequestDataAsync(stopwatch, context, searchResult, result.IsSuccess, result.ErrorReason).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                // Logging any error that occur and that are not being handled by the error handlers.
                _logger.Log("UnHandledErrors", "Command handler error: " + e);
            }
        }


        /// <summary>
        /// Checks if the message contains a prefix.
        /// </summary>
        /// <param name="logMessage">The log message that will be logged to CommandService/ErrorDetails</param>
        private async Task CommandServiceLogAsync(LogMessage logMessage)
        {

            // If log message is a CommandException, send a embedded error message.
            // Warning: This will only be activated if an error occured while running the command.
            if (logMessage.Exception is CommandException commandException)
            {
                var embed = await _commandErrorHandler.HandleErrorsAsync(commandException).ConfigureAwait(false);
                if (embed != null) await commandException.Context.Channel.SendMessageAsync("", false, embed.Build()).ConfigureAwait(false);
                _logger.Log("CommandService/ErrorDetails", $"Message: {logMessage.Message}, exception: {logMessage.Exception}, source: {logMessage.Source}");
            }
        }


        /// <summary>
        /// Saves the request data to the database.
        /// </summary>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        private async Task SaveRequestDataAsync(Stopwatch stopwatch, ShardedCommandContext context, SearchResult searchResult, bool isSuccessFul, string errorMessage = "")
        {
            using (var unitOfWork = Unity.Resolve<IRequestUnitOfWork>())
            {
                await unitOfWork.Requests.AddAsync(new Request
                {
                    Command = searchResult.Commands.FirstOrDefault().Command.Name,
                    ErrorMessage = errorMessage,
                    IsSuccessFull = isSuccessFul,
                    RunTime = stopwatch.ElapsedMilliseconds,
                    ServerId = context.Guild?.Id,
                    UserId = context.User.Id,
                    TimeStamp = DateTime.Now
                }).ConfigureAwait(false);
                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }
        }
    }
}
