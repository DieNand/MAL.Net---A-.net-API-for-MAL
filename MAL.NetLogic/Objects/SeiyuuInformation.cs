using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class SeiyuuInformation : ISeiyuuInformation
    {
        public string PictureUrl { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Language { get; set; }
    }
}