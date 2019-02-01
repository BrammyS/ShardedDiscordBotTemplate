using System.Threading.Tasks;

namespace Bot.Interfaces.Discord.EventHandlers.CommandHandlers
{
    public interface ICommandHandler
    {


        /// <summary>
        /// Initializes the command handler.
        /// </summary>
        Task InitializeAsync();
    }
}
