using System;
using System.Collections.Generic;
using MAL.NetLogic.Interfaces;
using Newtonsoft.Json;

namespace MAL.NetLogic.Objects
{
    public class AnimeOriginalJson : IAnimeOriginalJson
    {
            #region Properties

            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }

            [JsonProperty(PropertyName = "title")]
            public string Title { get; set; }

            [JsonProperty(PropertyName = "synopsis")]
            public string Synopsis { get; set; }

            [JsonProperty(PropertyName = "other_titles")]
            public Dictionary<string, List<string>> OtherTitles { get; set; }

            [JsonProperty(PropertyName = "summary_stats")]
            public List<string> SummaryStats { get; set; }

            [JsonProperty(PropertyName = "score_stats")]
            public List<string> ScoreStats { get; set; }

            [JsonProperty(PropertyName = "type")]
            public string Type { get; set; }

            [JsonProperty(PropertyName = "episodes")]
            public int? Episodes { get; set; }

            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }

            [JsonProperty(PropertyName = "classification")]
            public string Classification { get; set; }

            [JsonProperty(PropertyName = "start_date")]
            public DateTime StartDate { get; set; }

            [JsonProperty(PropertyName = "end_date")]
            public DateTime EndDate { get; set; }

            [JsonProperty(PropertyName = "popularity_rank")]
            public int Popularity { get; set; }

            [JsonProperty(PropertyName = "rank")]
            public int Rank { get; set; }

            [JsonProperty(PropertyName = "image_url")]
            public string ImageUrl { get; set; }

            [JsonProperty(PropertyName = "high_res_image_url")]
            public string HighResImageUrl { get; set; }

            [JsonProperty(PropertyName = "members_score")]
            public double MemberScore { get; set; }

            [JsonProperty(PropertyName = "members_count")]
            public int MemberCount { get; set; }

            [JsonProperty(PropertyName = "favorited_count")]
            public int FavoriteCount { get; set; }

            [JsonProperty(PropertyName = "watched_status")]
            public string UserWatchedStatus { get; set; }

            [JsonProperty(PropertyName = "watched_episodes")]
            public int UserWatchedEpisodes { get; set; }

            [JsonProperty(PropertyName = "score")]
            public int UserScore { get; set; }

            [JsonProperty(PropertyName = "genres")]
            public List<string> Genres { get; set; }

            [JsonProperty(PropertyName = "tags")]
            public List<string> Tags { get; set; }

            [JsonProperty(PropertyName = "additional_info_urls")]
            public InfoUrls AdditionalInfoUrls { get; set; }

            [JsonProperty(PropertyName = "manga_adaptations")]
            public List<IRelated> MangaAdaptation { get; set; }

            [JsonProperty(PropertyName = "prequels")]
            public List<IRelated> Prequels { get; set; }

            [JsonProperty(PropertyName = "sequels")]
            public List<IRelated> Sequels { get; set; }

            [JsonProperty(PropertyName = "side_stories")]
            public List<IRelated> SideStories { get; set; }

            [JsonProperty(PropertyName = "parent_story")]
            public IRelated ParentStory { get; set; }

            [JsonProperty(PropertyName = "character_anime")]
            public List<IRelated> CharacterAnime { get; set; }

            [JsonProperty(PropertyName = "spin_offs")]
            public List<IRelated> SpinOffs { get; set; }

            [JsonProperty(PropertyName = "summaries")]
            public List<IRelated> Summaries { get; set; }

            [JsonProperty(PropertyName = "alternative_versions")]
            public List<IRelated> AlternativeVersion { get; set; }

            [JsonProperty(PropertyName = "alternative_settings")]
            public List<IRelated> AlternativeSetting { get; set; }

            [JsonProperty(PropertyName = "full_stories")]
            public List<IRelated> FullStories { get; set; }
            
            [JsonProperty(PropertyName = "others")]
            public List<IRelated> Others { get; set; }

            [JsonProperty(PropertyName = "character_voice_actors")]
            public List<ICharacterInformation> CharacterInformation { get; set; }

            [JsonProperty(PropertyName = "error_occured")]
            public bool ErrorOccured { get; set; }

            [JsonProperty(PropertyName = "error_message")]
            public string ErrorMessage { get; set; }

            #endregion

            #region Constructor

            public AnimeOriginalJson()
            {
                OtherTitles = new Dictionary<string, List<string>> { { "japanese", new List<string>()}, {"english", new List<string>()}, { "synonyms", new List<string>()} };
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