using System;
using System.Collections.Generic;

namespace MAL.Net.Objects
{
    public class Anime
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
            ErrorOccured = false;
        }

        #endregion
    }
}