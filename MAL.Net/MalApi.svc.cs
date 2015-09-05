using MAL.NetLogic.Classes;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;

namespace MAL.Net
{
    public class MalApi : IMalApi
    {
        public IAnime GetAnime(int animeId)
        {
            return new Anime();
            //var retriever = new AnimeRetriever();
            //return retriever.GetAnime(animeId);
        }
    }
}
