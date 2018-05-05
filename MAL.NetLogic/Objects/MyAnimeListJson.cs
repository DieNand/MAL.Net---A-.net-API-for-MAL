using System.Collections.Generic;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class MyAnimeListJson : IMyAnimeListJson
    {
        #region Properties

        public IMyInfoJson Info { get; set; }
        public List<IListAnimeJson> Anime { get; set; }

        #endregion 
    }
}