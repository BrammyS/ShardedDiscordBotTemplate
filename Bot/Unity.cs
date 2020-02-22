using Bot.BotLists.Interfaces.Services;
using Bot.BotLists.Services;
using Bot.Discord;
using Bot.Discord.Configurations;
using Bot.Discord.Handlers;
using Bot.Discord.Handlers.CommandHandlers;
using Bot.Discord.Services;
using Bot.Discord.Timers;
using Bot.Interfaces;
using Bot.Interfaces.Discord;
using Bot.Interfaces.Discord.Handlers;
using Bot.Interfaces.Discord.Handlers.CommandHandlers;
using Bot.Interfaces.Discord.Services;
using Bot.Logger.Interfaces;
using Bot.Persistence.EntityFrameWork;
using Bot.Persistence.EntityFrameWork.Repositories;
using Bot.Persistence.EntityFrameWork.UnitOfWorks;
using Bot.Persistence.Repositories;
using Bot.Persistence.UnitOfWorks;
using Discord.Commands;
using Discord.WebSocket;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

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


            // DI for discord
            container.RegisterFactory<DiscordSocketConfig>(i => SocketConfig.GetDefault(), new SingletonLifetimeManager());
            container.RegisterFactory<CommandService>(i => CommandConfig.GetDefault(), new SingletonLifetimeManager());
            container.RegisterSingleton<DiscordShardedClient>(new InjectionConstructor(typeof(DiscordSocketConfig)));
            container.RegisterSingleton<IClientLogHandler, ClientLogHandler>();
            container.RegisterSingleton<IMiscEventHandler, MiscEventHandler>();
            container.RegisterSingleton<IPrefixService, PrefixService>();
            container.RegisterSingleton<IBotListUpdater, BotListUpdater>();
            container.RegisterSingleton<DiscordBotListsUpdateTimer>();

            container.RegisterType<ICommandErrorHandler, CommandErrorHandler>(new PerThreadLifetimeManager());
            container.RegisterType<ICommandInputErrorHandler, CommandInputErrorHandler>(new PerThreadLifetimeManager());
            container.RegisterType<ICommandHandler, CommandHandler>(new PerThreadLifetimeManager());
            container.RegisterType<ISpamFilter, SpamFilter>(new PerThreadLifetimeManager());


            // DI for Entity framework
            container.RegisterType<BotContext>(new PerResolveLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            container.RegisterType<IRequestUnitOfWork, RequestUnitOfWork>(new PerResolveLifetimeManager());
            container.RegisterType<IServerUnitOfWork, ServerUnitOfWork>(new PerResolveLifetimeManager());
            container.RegisterType<IServerRepository, ServerRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IRequestRepository, RequestRepository>(new PerResolveLifetimeManager());
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
