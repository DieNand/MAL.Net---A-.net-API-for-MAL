using System.Threading.Tasks;

namespace MAL.NetLogic.Interfaces
{
    public interface IAnimeListRetriever
    {
        Task<IMyAnimeList> GetAnimeList(string username);
    }
}