using System;
using System.Collections.Generic;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Objects
{
    public class Anime : IAnime
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }

        public List<string> JapaneseTitles { get; set; }
        public List<string> EnglishTitles { get; set; }
        public List<string> SynonymousTitles { get; set; }

        public List<string> SummaryStats { get; set; }
        public List<string> ScoreStats { get; set; }  

        public string Type { get; set; }
        public int? Episodes { get; set; }
        public string Status { get; set; }
        public string Classification { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Popularity { get; set; }
        public int Rank { get; set; }
        public string ImageUrl { get; set; }
        public string HighResImageUrl { get; set; }

        public double MemberScore { get; set; }
        public int MemberCount { get; set; }
        public int FavoriteCount { get; set; }

        public string UserWatchedStatus { get; set; }
        public int UserWatchedEpisodes { get; set; }
        public int UserScore { get; set; }

        public List<string> Genres { get; set; } 
        public List<string> Tags { get; set; } 

        public IInfoUrls AdditionalInfoUrls { get; set; }

        public List<IRelated> MangaAdaptation { get; set; }
        public List<IRelated> Prequels { get; set; }
        public List<IRelated> Sequels { get; set; }
        public List<IRelated> SideStories { get; set; }
        public IRelated ParentStory { get; set; }
        public List<IRelated> CharacterAnime { get; set; }
        public List<IRelated> SpinOffs { get; set; }
        public List<IRelated> Summaries { get; set; }
        public List<IRelated> AlternativeVersion { get; set; }
        public List<IRelated> Others { get; set; } 

        public bool ErrorOccured { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Constructor

        public Anime()
        {
            JapaneseTitles = new List<string>();
            EnglishTitles = new List<string>();
            SynonymousTitles = new List<string>();
            SummaryStats = new List<string>();
            ScoreStats = new List<string>();
            Genres = new List<string>();
            Tags = new List<string>();
            AdditionalInfoUrls = new InfoUrls();
            MangaAdaptation = new List<IRelated>();
            Prequels = new List<IRelated>();
            Sequels = new List<IRelated>();
            SideStories = new List<IRelated>();
            ParentStory = null;
            CharacterAnime = new List<IRelated>();
            SpinOffs = new List<IRelated>();
            Summaries = new List<IRelated>();
            AlternativeVersion = new List<IRelated>();
            Others = new List<IRelated>();
            ErrorOccured = false;
        }

        #endregion
    }
}