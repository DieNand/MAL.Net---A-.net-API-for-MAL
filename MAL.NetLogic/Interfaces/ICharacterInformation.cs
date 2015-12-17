using System.Collections.Generic;

namespace MAL.NetLogic.Interfaces
{
    public interface ICharacterInformation
    {
        string CharacterPicture { get; set; }
        string CharacterName { get; set; }
        string CharacterUrl { get; set; }
        string CharacterType { get; set; }
        List<ISeiyuuInformation> Seiyuu { get; set; }
    }
}