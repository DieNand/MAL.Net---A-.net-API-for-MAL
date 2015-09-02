using System.Collections.Generic;

namespace MAL.Net.Objects
{
    public class Anime
    {
        #region Properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int? Episodes { get; set; }

        public string Synopsis { get; set; }
        public List<string> JapaneseTitles { get; set; }
        public List<string> EnglishTitles { get; set; }
        public List<string> SynonymousTitles { get; set; }

        public bool ErrorOccured { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Constructor

        public Anime()
        {
            JapaneseTitles = new List<string>();
            EnglishTitles = new List<string>();
            SynonymousTitles = new List<string>();
            ErrorOccured = false;
        }

        #endregion
    }
}