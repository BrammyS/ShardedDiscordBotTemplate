using Bot.Discord;
using Bot.Discord.Configurations;
using Bot.Discord.Handlers;
using Bot.Discord.Handlers.CommandHandlers;
using Bot.Interfaces;
using Bot.Interfaces.Discord;
using Bot.Interfaces.Discord.EventHandlers;
using Bot.Interfaces.Discord.EventHandlers.CommandHandlers;
using Bot.Logger.Interfaces;
using Discord.Commands;
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

        /// <summary>
        /// Registers objects to the <see cref="container"/>
        /// </summary>
        public static void RegisterTypes()
        {
            container = new UnityContainer();
            container.RegisterType<IBot, Bot>(new PerThreadLifetimeManager());
            container.RegisterSingleton<IConnection, Connection>();

            container.RegisterType<ILogger, Logger.Logger>(new PerThreadLifetimeManager());


            //DI for discord
            container.RegisterSingleton<DiscordSocketConfig>(new InjectionFactory(i => SocketConfig.GetDefault()));
            container.RegisterSingleton<DiscordShardedClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            container.RegisterSingleton<CommandService>(new InjectionFactory(i => CommandConfig.GetDefault()));
            container.RegisterSingleton<IClientLogHandler, ClientLogHandler>();

            container.RegisterType<ICommandErrorHandler, CommandErrorHandler>(new PerThreadLifetimeManager());
            container.RegisterType<ICommandInputErrorHandler, CommandInputErrorHandler>(new PerThreadLifetimeManager());
            container.RegisterType<ICommandHandler, CommandHandler>(new PerThreadLifetimeManager());
        }


        /// <summary>
        /// Resolve a objects that is registered in the <see cref="container"/>.
        /// </summary>
        /// <typeparam name="T">The object you want to resolve</typeparam>
        /// <returns>The resolved object.</returns>
        public static T Resolve<T>()
        {
            return (T)Container.Resolve(typeof(T));
        }
    }
}
