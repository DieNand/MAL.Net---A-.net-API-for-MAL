using MAL.NetLogic.Interfaces;
using SimpleInjector;

namespace MAL.NetLogic.Factories
{
    public class AnimeFactory : IAnimeFactory
    {
        #region Variables

        private readonly Container _container;

        #endregion

        #region Constructor

        public AnimeFactory(Container container)
        {
            _container = container;
        }

        #endregion

        #region Methods

        public IAnime CreateAnime()
        {
            return _container.GetInstance<IAnime>();
        }

        public IAnimeOriginalJson CreateJsonAnime()
        {
            return _container.GetInstance<IAnimeOriginalJson>();
        }

        public IAnimeDetails CreateAnimeDetails()
        {
            return _container.GetInstance<IAnimeDetails>();
        }

        public IAnimeDetailsJson CreateJsonAnimeDetails()
        {
            return _container.GetInstance<IAnimeDetailsJson>();
        }

        public IMyAnimeList CreateAnimeList()
        {
            return _container.GetInstance<IMyAnimeList>();
        }

        public IMyAnimeListJson CreateJsonAnimeList()
        {
            return _container.GetInstance<IMyAnimeListJson>();
        }

        public IListAnime CreateListAnime()
        {
            return _container.GetInstance<IListAnime>();
        }

        public IMyInfo CreateMyInfo()
        {
            return _container.GetInstance<IMyInfo>();
        }

        public IAnimeDetailsXml CreateAnimeDetailsXml()
        {
            return _container.GetInstance<IAnimeDetailsXml>();
        }

        #endregion
    }
}