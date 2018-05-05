using System.Collections.Generic;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class CharacterInformation : ICharacterInformation
    {
        public string CharacterPicture { get; set; }
        public string CharacterName { get; set; }
        public string CharacterUrl { get; set; }
        public string CharacterType { get; set; }
        public List<ISeiyuuInformation> Seiyuu { get; set; }

        public CharacterInformation()
        {
            Seiyuu = new List<ISeiyuuInformation>();
        }
    }
}