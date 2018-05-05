using System.Collections.Generic;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class Character : ICharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Biography { get; set; }
        public int FavoriteCount { get; set; }
        public string Url { get; set; }
        public List<IAnimeography> Anime { get; set; }
        public List<IMangaography> Manga { get; set; }
        public string ErrorMessage { get; set; }
        public bool ErrorOccured { get; set; }

        public Character()
        {
            Anime = new List<IAnimeography>();
            Manga = new List<IMangaography>();
        }
    }
}