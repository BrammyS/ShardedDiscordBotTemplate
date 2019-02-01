using System.Collections.Concurrent;
using System.Threading.Tasks;
using Bot.Interfaces.Discord.Services;
using Bot.Logger.Interfaces;
using Bot.Persistence.UnitOfWorks;

namespace Bot.Discord.Services
{
    public class PrefixService : IPrefixService
    {
        public ILogger Logger { get; }


        private static readonly ConcurrentDictionary<ulong, string> ServerPrefixes = new ConcurrentDictionary<ulong, string>();

        public PrefixService(ILogger logger)
        {
            Logger = logger;
        }

        public void StorePrefix(string prefix, ulong key)
        {
            if (ServerPrefixes.ContainsKey(key))
            {
                var oldPrefix = GetPrefix(key);
                if (!ServerPrefixes.TryUpdate(key, prefix, oldPrefix)) Logger.Log($"Failed to update custom prefix {prefix} with the key: {key} from the dictionary");
                return;
            }

            if (!ServerPrefixes.TryAdd(key, prefix)) Logger.Log($"Failed to add custom prefix {prefix} with the key: {key} from the dictionary");
        }

        public string GetPrefix(ulong key)
        {
            return !ServerPrefixes.ContainsKey(key) ? null : ServerPrefixes[key];
        }

        public void RemovePrefix(ulong key)
        {
            if (!ServerPrefixes.ContainsKey(key)) return;
            if (!ServerPrefixes.TryRemove(key, out var removedPrefix)) Logger.Log($"Failed to remove custom prefix {removedPrefix} with the key: {key} from the dictionary");
        }

        public async Task LoadAllPrefixes()
        {
            using (var unitOfWork = Unity.Resolve<IServerUnitOfWork>())
            {
                var serverPrefixes = await unitOfWork.Servers.GetAllPrefixesAsync().ConfigureAwait(false);
                foreach (var serverPrefix in serverPrefixes) StorePrefix(serverPrefix.Prefix, serverPrefix.ServerId);
            }
        }

    }
}
