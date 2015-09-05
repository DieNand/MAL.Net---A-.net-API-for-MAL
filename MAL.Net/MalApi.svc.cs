using MAL.NetLogic.Classes;
using MAL.NetLogic.Objects;

namespace MAL.Net
{
    public class MalApi : IMalApi
    {
        public Anime GetAnime(int animeId)
        {
            var retriever = new AnimeRetriever();
            return retriever.GetAnime(animeId);
        }
    }
}
