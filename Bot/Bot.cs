using System.Threading.Tasks;
using Bot.Interfaces;
using Bot.Interfaces.Discord;

namespace Bot
{
    public class Bot : IBot
    {
        private readonly IConnection _connection;


        /// <summary>
        /// Creates a new <see cref="Bot"/>.
        /// </summary>
        /// <param name="connection">The <see cref="IConnection"/> that will be used.</param>
        public Bot(IConnection connection)
        {
            _connection = connection;
        }


        /// <inheritdoc />
        public Task StartAsync()
        {
            return _connection.ConnectAsync();
        }
    }
}
