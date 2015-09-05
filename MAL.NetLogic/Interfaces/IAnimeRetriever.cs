using MAL.NetLogic.Objects;

namespace MAL.NetLogic.Interfaces
{
    public interface IAnimeRetriever
    {
        IAnime GetAnime(int animeId);
    }
}