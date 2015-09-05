using System;
using System.Collections.Generic;
using MAL.NetLogic.Objects;

namespace MAL.NetLogic.Interfaces
{
    public interface IAnime
    {
        int Id { get; set; }
        string Title { get; set; }
        string Synopsis { get; set; }
        List<string> JapaneseTitles { get; set; }
        List<string> EnglishTitles { get; set; }
        List<string> SynonymousTitles { get; set; }
        List<string> SummaryStats { get; set; }
        List<string> ScoreStats { get; set; }
        string Type { get; set; }
        int? Episodes { get; set; }
        string Status { get; set; }
        string Classification { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        int Popularity { get; set; }
        int Rank { get; set; }
        string ImageUrl { get; set; }
        string HighResImageUrl { get; set; }
        double MemberScore { get; set; }
        int MemberCount { get; set; }
        int FavoriteCount { get; set; }
        string UserWatchedStatus { get; set; }
        int UserWatchedEpisodes { get; set; }
        int UserScore { get; set; }
        List<string> Genres { get; set; }
        List<string> Tags { get; set; }
        IInfoUrls AdditionalInfoUrls { get; set; }
        List<IRelated> MangaAdaptation { get; set; }
        List<IRelated> Prequels { get; set; }
        List<IRelated> Sequels { get; set; }
        List<IRelated> SideStories { get; set; }
        IRelated ParentStory { get; set; }
        List<IRelated> CharacterAnime { get; set; }
        List<IRelated> SpinOffs { get; set; }
        List<IRelated> Summaries { get; set; }
        List<IRelated> AlternativeVersion { get; set; }
        List<IRelated> Others { get; set; }
        bool ErrorOccured { get; set; }
        string ErrorMessage { get; set; }
    }
}