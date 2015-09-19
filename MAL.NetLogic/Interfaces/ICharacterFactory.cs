namespace MAL.NetLogic.Interfaces
{
    public interface ICharacterFactory
    {
        ICharacterInformation CreateCharacter();
        ISeiyuuInformation CreateSeiyuu();
    }
}