using Bot.Discord;
using Bot.Discord.Configurations;
using Bot.Discord.EventHandlers;
using Bot.Interfaces;
using Bot.Interfaces.Discord;
using Bot.Interfaces.Discord.EventHandlers;
using Bot.Logger.Interfaces;
using Discord.WebSocket;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Resolution;

namespace Bot
{
    public static class Unity
    {
        private static UnityContainer container;

        private static UnityContainer Container
        {
            get
            {
                if (container == null)
                    RegisterTypes();
                return container;
            }
        }

        public static void RegisterTypes()
        {
            container = new UnityContainer();
            container.RegisterType<IBot, Bot>(new PerThreadLifetimeManager());

            container.RegisterType<ILogger, Logger.Logger>(new PerThreadLifetimeManager());

            container.RegisterSingleton<DiscordSocketConfig>(new InjectionFactory(i => SocketConfig.GetDefault()));
            container.RegisterSingleton<DiscordShardedClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            container.RegisterSingleton<IConnection, Connection>();
            container.RegisterSingleton<IClientLogHandler, ClientLogHandler>();
        }

        public static T Resolve<T>()
        {
            return (T)Container.Resolve(typeof(T), string.Empty, new CompositeResolverOverride());
        }
    }
}
