using System.ServiceModel;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;

namespace MAL.Net
{
    [ServiceContract]
    public interface IMalApi
    {

        [OperationContract]
        IAnime GetAnime(int animeId);
    }
}
