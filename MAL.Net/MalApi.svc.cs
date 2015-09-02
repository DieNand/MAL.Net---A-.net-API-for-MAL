using MAL.Net.Classes;
using MAL.Net.Objects;

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
