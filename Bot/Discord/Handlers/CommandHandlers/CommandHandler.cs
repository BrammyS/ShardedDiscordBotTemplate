using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Bot.Interfaces.Discord.EventHandlers.CommandHandlers;
using Bot.Logger.Interfaces;
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
        private readonly ICommandErrorHandler _commandErrorHandler;
        private readonly ICommandInputErrorHandler _commandInputErrorHandler;
        private IServiceProvider _services;

        public CommandHandler(DiscordShardedClient client, CommandService commandService, ILogger logger, ICommandErrorHandler commandErrorHandler, ICommandInputErrorHandler commandInputErrorHandler)
        {
            _client = client;
            _commandService = commandService;
            _logger = logger;
            _commandErrorHandler = commandErrorHandler;
            _commandInputErrorHandler = commandInputErrorHandler;
        }

        public async Task InitializeAsync()
        {
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commandService)
                .AddScoped<ILogger, Logger.Logger>()
                .BuildServiceProvider();
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services).ConfigureAwait(false);
            _commandService.Log += LogCommandServiceEvent;
            _client.MessageReceived += HandleCommandEvent;
        }


        private Task LogCommandServiceEvent(LogMessage logMessage)
        {
            Task.Run(async () => await CommandServiceLogAsync(logMessage).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private Task HandleCommandEvent(SocketMessage message)
        {
            if (!(message is SocketUserMessage msg)) return Task.CompletedTask;
            Task.Run(async () => await CheckForPrefixAsync(msg).ConfigureAwait(false));
            return Task.CompletedTask;
        }

        private async Task CheckForPrefixAsync(SocketUserMessage msg)
        {
            if (msg.Author.IsBot) return;
            if (msg.Channel.Id == 265156361791209475) return;
            var context = new ShardedCommandContext(_client, msg);
            var argPos = 0;
            if (context.Message.HasStringPrefix("!", ref argPos, StringComparison.CurrentCultureIgnoreCase) ||
                context.Message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                await HandleCommandAsync(context, argPos).ConfigureAwait(false);
            }
        }

        private async Task HandleCommandAsync(ShardedCommandContext context, int argPos)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var searchResult = _commandService.Search(context, argPos);
                if (searchResult.Commands == null || searchResult.Commands.Count == 0)
                {
                    stopwatch.Stop();
                    return;
                }
                var result = await _commandService.ExecuteAsync(context, argPos, _services).ConfigureAwait(false);
                if (!result.IsSuccess)
                {
                    _logger.Log("Error", result.ErrorReason, context.Message.Content, context.User.Username, context.Guild.Name, context.Guild.Id);
                    var embed = await _commandInputErrorHandler.HandleErrorsAsync(result, context).ConfigureAwait(false);
                    if (embed != null) await context.Channel.SendMessageAsync("", false, embed.Build()).ConfigureAwait(false);
                }
                stopwatch.Stop();
            }
            catch (Exception e)
            {
                _logger.Log("UnHandledErrors", "Command handler error: " + e);
            }
        }

        private async Task CommandServiceLogAsync(LogMessage logMessage)
        {
            if (logMessage.Exception is CommandException commandException)
            {
                var embed = await _commandErrorHandler.HandleErrorsAsync(commandException).ConfigureAwait(false);
                if (embed != null) await commandException.Context.Channel.SendMessageAsync("", false, embed.Build()).ConfigureAwait(false);
            }
            _logger.Log("CommandService/ErrorDetails", $"Message: {logMessage.Message}, exception: {logMessage.Exception}, source: {logMessage.Source}");
        }
    }
}
