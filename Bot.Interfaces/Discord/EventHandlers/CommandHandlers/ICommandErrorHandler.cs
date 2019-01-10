using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Bot.Interfaces.Discord.EventHandlers.CommandHandlers
{
    public interface ICommandErrorHandler
    {
        Task<EmbedBuilder> HandleErrorsAsync(CommandException commandException);

    }
}
