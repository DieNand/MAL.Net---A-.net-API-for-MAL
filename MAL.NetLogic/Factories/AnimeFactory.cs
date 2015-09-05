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

        #endregion
    }
}