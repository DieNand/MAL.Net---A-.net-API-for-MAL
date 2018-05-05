using MAL.NetLogic.Interfaces;
using SimpleInjector;

namespace MAL.NetLogic.Factories
{
    public class SeasonFactory : ISeasonFactory
    {
        #region Variables

        private readonly Container _container;

        #endregion

        #region Constructor

        public SeasonFactory(Container container)
        {
            _container = container;
        }

        #endregion

        #region Public Methods

        public ISeasonData CreateSeasonData()
        {
            return _container.GetInstance<ISeasonData>();
        }

        #endregion
    }
}