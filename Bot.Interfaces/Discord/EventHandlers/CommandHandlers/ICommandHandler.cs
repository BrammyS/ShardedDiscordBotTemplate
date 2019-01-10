using System.Threading.Tasks;

namespace Bot.Interfaces.Discord.EventHandlers.CommandHandlers
{
    public interface ICommandHandler
    {
        Task InitializeAsync();

    }
}
