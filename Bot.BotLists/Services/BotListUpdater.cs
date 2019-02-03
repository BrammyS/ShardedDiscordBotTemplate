using System.Threading.Tasks;
using Bot.BotLists.Interfaces.Services;

namespace Bot.BotLists.Services
{
    public class BotListUpdater : IBotListUpdater
    {

        /// <inheritdoc />
        public Task UpdateStatusAsync(int shardCount, int guildCount, int shardId = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
