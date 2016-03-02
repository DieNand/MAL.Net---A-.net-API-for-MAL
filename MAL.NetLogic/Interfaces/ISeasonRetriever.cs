using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAL.NetLogic.Interfaces
{
    public interface ISeasonRetriever
    {
        Task<List<ISeasonData>> GetSeasonData(int year, string season);
    }
}