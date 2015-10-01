using System.Threading.Tasks;

namespace MAL.NetLogic.Interfaces
{
    public interface IDataPush
    {
        Task<bool> PushAnimeDetailsToMal(IAnimeDetailsJson details, string username, string password, bool canCache = true);
    }
}