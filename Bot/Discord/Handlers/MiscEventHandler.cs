using System.Threading.Tasks;
using Bot.Interfaces.Discord.Handlers;
using Bot.Persistence.UnitOfWorks;
using Discord.WebSocket;

namespace Bot.Discord.Handlers
{
    public class MiscEventHandler : IMiscEventHandler
    {
        private readonly DiscordShardedClient _client;


        public MiscEventHandler(DiscordShardedClient client)
        {
            _client = client;
        }


        /// <inheritdoc />
        public void Initialize()
        {
            _client.JoinedGuild += JoinedGuildEvent;
            _client.LeftGuild += LeftGuildEvent;
        }


        /// <summary>
        /// Handles the <see cref="BaseSocketClient.LeftGuild"/> event.
        /// </summary>
        /// <param name="guild">The guild that the client left.</param>
        private Task LeftGuildEvent(SocketGuild guild)
        {
            Task.Run(async () => await LeftGuild(guild).ConfigureAwait(false));
            return Task.CompletedTask;
        }


        /// <summary>
        /// Handles the <see cref="BaseSocketClient.JoinedGuild"/> event.
        /// </summary>
        /// <param name="guild">The guild that the client joined.</param>
        private Task JoinedGuildEvent(SocketGuild guild)
        {
            Task.Run(async () => await JoinedGuild(guild).ConfigureAwait(false));
            return Task.CompletedTask;
        }


        /// <summary>
        /// Adds the server to the database.
        /// and sets it to Active.
        /// </summary>
        /// <param name="guild">The guild that the client joined.</param>
        private async Task JoinedGuild(SocketGuild guild)
        {

            using (var unitOfWork = Unity.Resolve<IServerUnitOfWork>())
            {
                var server = await unitOfWork.Servers.GetOrAddServerAsync(guild.Id, guild.Name, guild.MemberCount).ConfigureAwait(false);
                server.Active = true;
                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }

        }


        /// <summary>
        /// Sets the server to inactive.
        /// </summary>
        /// <param name="guild">The guild that the client left.</param>
        private async Task LeftGuild(SocketGuild guild)
        {
        
            using (var unitOfWork = Unity.Resolve<IServerUnitOfWork>())
            {
                var server = await unitOfWork.Servers.GetOrAddServerAsync(guild.Id, guild.Name, guild.MemberCount).ConfigureAwait(false);
                server.Active = false;
                await unitOfWork.SaveAsync().ConfigureAwait(false);
            }

        }
    }
}

