using System.Threading.Tasks;
using MAL.NetLogic.Objects;

namespace MAL.NetLogic.Interfaces
{
    public interface IAnimeRetriever
    {
        Task<IAnime> GetAnime(int animeId);
    }
}