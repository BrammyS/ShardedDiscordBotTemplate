using System.Threading.Tasks;

namespace Bot.Interfaces.Discord.Handlers.CommandHandlers
{
    public interface ICommandHandler
    {


        /// <summary>
        /// Initializes the command handler.
        /// </summary>
        Task InitializeAsync();
    }
}
