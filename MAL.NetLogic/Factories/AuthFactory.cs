using MAL.NetLogic.Interfaces;
using SimpleInjector;

namespace MAL.NetLogic.Factories
{
    public class AuthFactory : IAuthFactory
    {
        #region Variables

        private readonly Container _container;

        #endregion

        #region Constructor

        public AuthFactory(Container container)
        {
            _container = container;
        }

        #endregion

        #region Methods

        public ILoginData CreateLingData()
        {
            return _container.GetInstance<ILoginData>();
        }

        #endregion
    }
}