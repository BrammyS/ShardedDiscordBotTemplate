using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Bot.Interfaces.Discord.Handlers.CommandHandlers
{
    public interface ICommandErrorHandler
    {


        /// <summary>
        /// Handles a error message when a error is thrown while using a command.
        /// </summary>
        /// <param name="commandException">The command exception of the command where the error occured.</param>
        /// <returns>The embedded error message.</returns>
        Task<EmbedBuilder> HandleErrorsAsync(CommandException commandException);
    }
}
