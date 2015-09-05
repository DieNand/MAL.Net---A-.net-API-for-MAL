using System.ServiceModel;
using MAL.NetLogic.Objects;

namespace MAL.Net
{
    [ServiceContract]
    public interface IMalApi
    {

        [OperationContract]
        Anime GetAnime(int animeId);
    }
}
