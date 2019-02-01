using System.Threading.Tasks;

namespace Bot.Interfaces.Discord.Services
{
    public interface IPrefixService
    {
        string GetPrefix(ulong key);
        void StorePrefix(string prefix, ulong key);
        void RemovePrefix(ulong key);
        Task LoadAllPrefixes();
    }
}
