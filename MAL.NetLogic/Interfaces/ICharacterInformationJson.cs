using System.Collections.Generic;

namespace MAL.NetLogic.Interfaces
{
    public interface ICharacterInformationJson
    {
        string CharacterPicture { get; set; }
        string CharacterName { get; set; }
        string CharacterUrl { get; set; }
        string CharacterType { get; set; }
        List<ISeiyuuInformationJson> Seiyuu { get; set; }
    }
}