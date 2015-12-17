using MAL.NetLogic.Interfaces;
using SimpleInjector;

namespace MAL.NetLogic.Factories
{
    public class WebHttpWebRequestFactory : IWebHttpWebRequestFactory
    {
        #region Variables

        private readonly Container _container;

        #endregion

        #region Constructor

        public WebHttpWebRequestFactory(Container container)
        {
            _container = container;
        }

        #endregion

        #region Public Methods

        public IWebHttpWebRequest Create()
        {
            return _container.GetInstance<IWebHttpWebRequest>();
        }

        #endregion
    }
}