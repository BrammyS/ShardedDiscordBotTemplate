using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Bot.Interfaces.Discord.Handlers.CommandHandlers
{
    public interface ICommandInputErrorHandler
    {


        /// <summary>
        /// Handles a error message when the input for a command is wrong.
        /// </summary>
        /// <param name="iResult">The Result information of the command.</param>
        /// <param name="context">The command context where the error occured.</param>
        /// <returns>The embedded error message.</returns>
        Task<EmbedBuilder> HandleErrorsAsync(IResult iResult, SocketCommandContext context);
    }
}
