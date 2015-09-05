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

        public string Type { get; set; }
        public int? Episodes { get; set; }
        public string Status { get; set; }
        public string Classification { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Popularity { get; set; }
        public int Ranked { get; set; }

        public InfoUrls AdditionalInfoUrls { get; set; }

        public List<Related> MangaAdaptation { get; set; }
        public List<Related> Prequels { get; set; }
        public List<Related> Sequels { get; set; }
        public List<Related> SideStories { get; set; }
        public Related ParentStory { get; set; }
        public List<Related> CharacterAnime { get; set; }
        public List<Related> SpinOffs { get; set; }
        public List<Related> Summaries { get; set; }
        public List<Related> AlternativeVersion { get; set; }
        public List<Related> Others { get; set; } 

        public bool ErrorOccured { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Constructor

        public Anime()
        {
            JapaneseTitles = new List<string>();
            EnglishTitles = new List<string>();
            SynonymousTitles = new List<string>();
            AdditionalInfoUrls = new InfoUrls();
            MangaAdaptation = new List<Related>();
            Prequels = new List<Related>();
            Sequels = new List<Related>();
            SideStories = new List<Related>();
            ParentStory = new Related();
            CharacterAnime = new List<Related>();
            SpinOffs = new List<Related>();
            Summaries = new List<Related>();
            AlternativeVersion = new List<Related>();
            Others = new List<Related>();
            ErrorOccured = false;
        }

        #endregion
    }

    public class InfoUrls
    {
        public string Episodes { get; set; }
        public string Reviews { get; set; }
        public string Recommendation { get; set; }
        public string Stats { get; set; }
        public string CharactersAndStaff { get; set; }
        public string News { get; set; }
        public string Forum { get; set; }
        public string Featured { get; set; }
        public string Clubs { get; set; }
        public string Pictures { get; set; }
    }

    public class Related
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}