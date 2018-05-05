using System.Collections.Generic;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class MyAnimeList : IMyAnimeList
    {
        #region Properties

        public IMyInfo Info { get; set; }
        public List<IListAnime> Anime { get; set; }

        #endregion

        #region Constructor

        public MyAnimeList()
        {
            Anime = new List<IListAnime>();
        }

        #endregion
    }
}