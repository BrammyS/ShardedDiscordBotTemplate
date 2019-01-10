using System.Threading.Tasks;

namespace Bot.Interfaces.Discord
{
    public interface IConnection
    {
        Task ConnectAsync();
    }
}
