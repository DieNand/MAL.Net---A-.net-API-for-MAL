using System.Collections.Generic;

namespace MAL.NetLogic.Interfaces
{
    public interface ICharacter
    {
        int Id { get; set; }
        string Name { get; set; }
        string ImageUrl { get; set; }
        string Biography { get; set; }
        int FavoriteCount { get; set; }
        string Url { get; set; }
        List<IAnimeography> Anime { get; set; }
        List<IMangaography> Manga { get; set; }
        string ErrorMessage { get; set; }
        bool ErrorOccured { get; set; }
    }
}