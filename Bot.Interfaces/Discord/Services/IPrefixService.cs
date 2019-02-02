using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Bot.Interfaces.Discord.Services
{
    public interface IPrefixService
    {

        /// <summary>
        /// Gets the custom prefix for that server.
        /// This returns Null if the server doesn't have a custom prefix.
        /// </summary>
        /// <param name="key">The Id of the server.</param>
        /// <returns></returns>
        string GetPrefix(ulong key);


        /// <summary>
        /// Stores a custom prefix for a servers.
        /// </summary>
        /// <param name="prefix">Prefix that will be saved.</param>
        /// <param name="key">The id of the server.</param>
        void StorePrefix(string prefix, ulong key);


        /// <summary>
        /// Removes the prefix from the server.
        /// </summary>
        /// <param name="key">The Id of the server.</param>
        void RemovePrefix(ulong key);


        /// <summary>
        /// Loads all the custom prefixes the a <see cref="ConcurrentDictionary{TKey,TValue}"/>
        /// </summary>
        /// <returns></returns>
        Task LoadAllPrefixes();
    }
}
