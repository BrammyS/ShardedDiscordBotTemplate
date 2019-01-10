using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Bot.Interfaces.Discord.EventHandlers.CommandHandlers
{
    public interface ICommandInputErrorHandler
    {
        Task<EmbedBuilder> HandleErrorsAsync(IResult iResult, SocketCommandContext context);
    }
}
