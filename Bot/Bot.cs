using System.Threading.Tasks;
using Bot.Interfaces;
using Bot.Interfaces.Discord;

namespace Bot
{
    public class Bot : IBot
    {
        private readonly IConnection _connection;

        public Bot(IConnection connection)
        {
            _connection = connection;
        }

        public async Task StartAsync()
        {
            await _connection.ConnectAsync().ConfigureAwait(false);
        }
    }
}
