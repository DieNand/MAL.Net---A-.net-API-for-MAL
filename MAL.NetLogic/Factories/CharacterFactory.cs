using MAL.NetLogic.Interfaces;
using SimpleInjector;

namespace MAL.NetLogic.Factories
{
    public class CharacterFactory : ICharacterFactory
    {
        #region Variables

        private readonly Container _container;

        #endregion

        #region Constructor

        public CharacterFactory(Container container)
        {
            _container = container;
        }

        #endregion

        #region Public Methods

        public ICharacterInformation CreateCharacter()
        {
            return _container.GetInstance<ICharacterInformation>();
        }

        public ISeiyuuInformation CreateSeiyuu()
        {
            return _container.GetInstance<ISeiyuuInformation>();
        }

        public ICharacterInformationJson CreateJsonCharacter()
        {
            return _container.GetInstance<ICharacterInformationJson>();
        }

        public ICharacter CreateFullCharacter()
        {
            return _container.GetInstance<ICharacter>();
        }

        #endregion
    }
}