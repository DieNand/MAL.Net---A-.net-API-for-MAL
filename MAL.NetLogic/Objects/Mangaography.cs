using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class Mangaography : IMangaography
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; } 
    }
}