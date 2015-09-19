using System.Collections.Generic;

namespace MAL.NetLogic.Interfaces
{
    public interface IMyAnimeListJson
    {
        IMyInfoJson Info { get; set; }
        List<IListAnimeJson> Anime { get; set; }
    }
}