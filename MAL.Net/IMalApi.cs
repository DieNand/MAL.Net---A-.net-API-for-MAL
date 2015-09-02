using System.ServiceModel;
using MAL.Net.Objects;

namespace MAL.Net
{
    [ServiceContract]
    public interface IMalApi
    {

        [OperationContract]
        Anime GetAnime(int animeId);
    }
}
