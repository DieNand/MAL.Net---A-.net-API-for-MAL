using System.Collections.Generic;
using MAL.NetLogic.Objects;

namespace MAL.NetLogic.Interfaces
{
    public interface IMyAnimeList
    {
        IMyInfo Info { get; set; }
        List<IListAnime> Anime { get; set; }
    }
}