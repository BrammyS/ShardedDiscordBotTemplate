using System.Linq;
using System.Threading.Tasks;
using Bot.Interfaces.Discord.Handlers.CommandHandlers;
using Bot.Interfaces.Discord.Services;
using Bot.Logger.Interfaces;
using Discord;
using Discord.Commands;

namespace Bot.Discord.Handlers.CommandHandlers
{
    public class CommandInputErrorHandler : ErrorHandler, ICommandInputErrorHandler
    {
        private readonly IPrefixService _prefixService;

        /// <summary>
        /// Creates a new <see cref="CommandInputErrorHandler"/>.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> that will be used to log all the messages.</param>
        /// <param name="prefixService"></param>
        public CommandInputErrorHandler(ILogger logger, IPrefixService prefixService) : base(logger)
        {
            _prefixService = prefixService;
        }


        /// <inheritdoc />
        public async Task<EmbedBuilder> HandleErrorsAsync(IResult iResult, SocketCommandContext context)
        {
            var message = context.Message.Content.ToLower();
            var prefix = Constants.Prefix;
            var result = iResult.ToString();

            if (result.Contains("UnmetPrecondition: Bot requires guild permission EmbedLinks"))
            {
                await context.Channel.SendMessageAsync("I need the EmbedLinks permission for that command!").ConfigureAwait(false);
                return null;
            }

            // User permission errors.
            if (result.Contains("UnmetPrecondition: User requires guild permission Administrator"))
                return EmbedError("Permission error", ":no_entry_sign: You need admin permissions for this command.");
            if (result.Contains("UnmetPrecondition: User requires guild permission ManageRoles"))
                return EmbedError("Permission error", ":no_entry_sign: You need Manage Roles permissions for this command.");

            // Sending the user a DM if the bot cant send a message in that channel.
            if (result.Contains("UnmetPrecondition: Bot requires guild permission SendMessages"))
            {
                await context.User.SendMessageAsync("", false, EmbedError("Permission error", "I need don't have the right permissions to talk in that channel!\n" +
                                                                                              "Pls give me the SendMessages permission!").Build()).ConfigureAwait(false);
                return null;
            }

            if (result.Contains("A quoted parameter is incomplete"))
                return EmbedError("Incorrect input", "If your are using quotes pls remember to close them <3");

            // If error is a Missing permissions error, send embedded error message.
            if (result.Contains("The server responded with error 50013: Missing Permissions") || result.Contains("UnmetPrecondition: Bot requires guild permission"))
            {
                var guildPermissions = context.Guild.CurrentUser.Roles.Select(x => x.Permissions).ToList();
                var description = "Im missing the following permissions:";
                if (!guildPermissions.Any(x => x.SendMessages)) description += " **SendMessages**";
                if (!guildPermissions.Any(x => x.EmbedLinks)) description += " **EmbedLinks**";
                if (!guildPermissions.Any(x => x.AddReactions)) description += " **AddReactions**";
                await context.Channel.SendMessageAsync(description).ConfigureAwait(false);
                return null;
            }


            // Checking what command what used to embed the correct error.
            var defaultPrefixErrorMessage = CheckForCommand(message, result, prefix);
            if (defaultPrefixErrorMessage != null) return defaultPrefixErrorMessage;
            var customPrefix = _prefixService.GetPrefix(context.Guild.Id);
            if (customPrefix != null)
            {
                var customPrefixErrorMessage = CheckForCommand(message, result, customPrefix);
                if (customPrefixErrorMessage != null) return customPrefixErrorMessage;
            }

            // To many or to few parameters error messages.
            if (result.Contains("BadArgCount: The input text has too many parameters."))
                return EmbedError("Incorrect input", "The input has to many parameters");
            if (result.Contains("BadArgCount: The input text has too few parameters."))
                return EmbedError("Incorrect input", "The input text has too few parameters.");

            return GetDefaultError(result);
        }


        /// <summary>
        /// Checks what command was used.
        /// </summary>
        /// <param name="message">The <see cref="string"/> that contains the message.</param>
        /// <param name="result">The <see cref="string"/> that contains the error message.</param>
        /// <param name="prefix">The <see cref="string"/> that contains the prefix.</param>
        /// <returns></returns>
        private EmbedBuilder CheckForCommand(string message, string result, string prefix)
        {
            if (message.Contains($"{prefix}botinfo")) return HandleBotInfoErrors(result, prefix);
            if (message.Contains($"{prefix}ping")) return HandlePingErrors(result, prefix);
            if (message.Contains($"{prefix}shards")) return HandleShardsErrors(result, prefix);
            return null;
        }


        /// <summary>
        /// Embeds the error messages for the BotInfo command.
        /// </summary>
        /// <param name="result">The <see cref="string"/> that contains the error message.</param>
        /// <param name="prefix">The <see cref="string"/> that contains the prefix.</param>
        /// <returns> A embedded error message.</returns>
        private EmbedBuilder HandleBotInfoErrors(string result, string prefix)
        {
            if (result.Contains("The input text has too many parameters."))
                return EmbedError("Incorrect input", "This command does not need any input!\n" +
                                                     $"Example: **{prefix}Botinfo**");
            return GetDefaultError(result);
        }


        /// <summary>
        /// Embeds the error messages for the Ping command.
        /// </summary>
        /// <param name="result">The <see cref="string"/> that contains the error message.</param>
        /// <param name="prefix">The <see cref="string"/> that contains the prefix.</param>
        /// <returns> A embedded error message.</returns>
        private EmbedBuilder HandlePingErrors(string result, string prefix)
        {
            if (result.Contains("The input text has too many parameters."))
                return EmbedError("Incorrect input", "This command does not need any input!\n" +
                                                     $"Example: **{prefix}Ping**");
            return GetDefaultError(result);
        }


        /// <summary>
        /// Embeds the error messages for the Shards command.
        /// </summary>
        /// <param name="result">The <see cref="string"/> that contains the error message.</param>
        /// <param name="prefix">The <see cref="string"/> that contains the prefix.</param>
        /// <returns> A embedded error message.</returns>
        private EmbedBuilder HandleShardsErrors(string result, string prefix)
        {
            if (result.Contains("The input text has too many parameters."))
                return EmbedError("Incorrect input", "This command does not need any input!\n" +
                                                     $"Example: **{prefix}Shards**");
            return GetDefaultError(result);
        }
    }
}
