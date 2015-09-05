using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using MAL.Net.Objects;

namespace MAL.Net.Classes
{
    public class AnimeRetriever
    {
        #region Variables

        private const string MalUrl = @"http://myanimelist.net/anime/{0}";
        private const string CleanMalUrl = @"http://myanimelist.net{0}";
        private readonly string _examplePath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

        #endregion

        public Anime GetAnime(int animeId)
        {
            var anime = new Anime();

            try
            {
                //Our first task is to retrieve the MAL anime - for now we cheat and grab it from our example data
                var doc = new HtmlDocument();
#if DEBUG
                var file = Path.Combine("AnimeExamples", $"{animeId}.html");
                doc.Load(Path.Combine(_examplePath, file));
#else
            doc.LoadHtml(string.Format(MalUrl, animeId));
#endif

                //Retrieve the MAL ID
                int aid;
                var idString =
                    doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @name='aid']").Attributes["value"]
                        .Value;
                int.TryParse(idString, out aid);
                anime.Id = aid;

                anime.Title =
                    doc.DocumentNode.SelectSingleNode("//h1").SelectSingleNode("//span[@itemprop='name']").InnerText;
                anime.Synopsis = doc.DocumentNode.SelectSingleNode("//span[@itemprop='description']").InnerText;
                //We need to do some synopsis cleanup

                //Retrieve Alternative titles
                foreach (var node in doc.DocumentNode.SelectNodes("//div[@class='spaceit_pad']"))
                {
                    var lang = node.ChildNodes.Descendants().FirstOrDefault()?.InnerText.Trim(':');

                    switch (lang)
                    {
                        case "Japanese":
                            var jTitle = node.ChildNodes["#text"].InnerText;
                            anime.JapaneseTitles.AddRange(jTitle.Split(',').Select(t => t.Trim()));
                            break;
                        case "English":
                            var eTitle = node.ChildNodes["#text"].InnerText;
                            anime.EnglishTitles.AddRange(eTitle.Split(',').Select(t => t.Trim()));
                            break;
                        case "Synonyms":
                            var sTitle = node.ChildNodes["#text"].InnerText;
                            anime.SynonymousTitles.AddRange(sTitle.Split(',').Select(t => t.Trim()));
                            break;
                    }
                }

                var img = doc.DocumentNode.SelectSingleNode("//img[@itemprop='image']").Attributes["src"].Value;
                anime.ImageUrl = img;
                anime.HighResImageUrl = img.Insert(img.Length - 4, "l");

                foreach (var node in doc.DocumentNode.SelectNodes("//div"))
                {
                    var innerSpan = node.ChildNodes.Descendants().FirstOrDefault()?.InnerText.Trim(':');
                    switch (innerSpan)
                    {
                        case "Type":
                            anime.Type = node.ChildNodes["#text"].InnerText.Trim();
                            break;
                        case "Episodes":
                            string epString;
                            epString = node.ChildNodes["#text"].InnerText;
                            int eps;
                            int.TryParse(epString, out eps);
                            if (eps == 0)
                            {
                                epString = node.ChildNodes[2].InnerText.Replace("\r\n", "").Trim();
                                int.TryParse(epString, out eps);
                                anime.Episodes = eps;
                            }

                            if(eps == 0)
                                anime.Episodes = null;
                            break;
                        case "Status":
                            anime.Status = node.ChildNodes["#text"].InnerText.Trim();
                            break;
                        case "Aired":
                            var dateString = node.ChildNodes["#text"].InnerText;
                            var dates = Regex.Split(dateString, " to ");
                            var startDate = DateTime.MinValue;
                            var endDate = DateTime.MinValue;
                            if(dates.Any())
                                DateTime.TryParse(dates[0], out startDate);
                            if(dates.Count() > 1)
                                DateTime.TryParse(dates[1], out endDate);
                            anime.StartDate = startDate;
                            anime.EndDate = endDate;
                            break;
                        case "Rating":
                            var txt = node.InnerText.Replace("\r\n", "");
                            var cleanText = Regex.Split(txt, "                                    ").Last().Trim();
                            anime.Classification = cleanText;
                            break;
                        case "Ranked":
                            var rankString = node.ChildNodes["#text"].InnerText.Trim().TrimStart('#');
                            int number;
                            int.TryParse(rankString, out number);
                            anime.Rank = number;
                            break;
                        case "Popularity":
                            var pString = node.ChildNodes["#text"].InnerText.Trim().TrimStart('#');
                            int pNum;
                            int.TryParse(pString, out pNum);
                            anime.Popularity = pNum;
                            break;
                        case "Score":
                            var scoreString = node.SelectNodes("//span[@itemprop='ratingValue']")[0].InnerText;
                            double scoreVal;
                            double.TryParse(scoreString, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out scoreVal);
                            anime.MemberScore = scoreVal;
                            break;
                        case "Members":
                            var memberString = node.ChildNodes["#text"].InnerText.Trim().Replace(",", "");
                            int mVal;
                            int.TryParse(memberString, out mVal);
                            anime.MemberCount = mVal;
                            break;
                        case "Favorites":
                            var favString = node.ChildNodes["#text"].InnerText.Trim();
                            int fVal;
                            int.TryParse(favString, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal);
                            anime.FavoriteCount = fVal;
                            break;
                        case "Genres":
                            foreach (var g in node.SelectNodes("//span[@itemprop='genre']"))
                            {
                                anime.Genres.Add(g.InnerText);
                            }
                            break;
                    }

                    foreach (var tagNode in doc.DocumentNode.SelectNodes("//div[@class='tags']"))
                    {
                        foreach (var tag in tagNode.ChildNodes.Nodes())
                        {
                            if(tag.OriginalName == "a" && !anime.Tags.Contains(tag.InnerText))
                                anime.Tags.Add(tag.InnerText);
                        }
                    }
                }
                GetInfoUrls(doc, anime);
                GetRelated(doc, anime);

            }
            catch (Exception ex)
            {
                anime.ErrorOccured = true;
                anime.ErrorMessage = ex.Message;
            }

            return anime;
        }

        #region Private Methods

        private void GetInfoUrls(HtmlDocument doc, Anime anime)
        {
            foreach (var listItem in doc.DocumentNode.SelectNodes("//div[@id='horiznav_nav']"))
            {
                foreach(var child in listItem.ChildNodes["ul"].ChildNodes)
                {
                    var item = child.ChildNodes["a"];
                    if(item == null) continue;
                    switch (item.InnerText)
                    {
                        case "Episodes":
                            anime.AdditionalInfoUrls.Episodes = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                        case "Reviews":
                            anime.AdditionalInfoUrls.Reviews = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                        case "Recommendations":
                            anime.AdditionalInfoUrls.Recommendation = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                        case "Stats":
                            anime.AdditionalInfoUrls.Stats = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                        case "Characters &amp; Staff":
                            anime.AdditionalInfoUrls.CharactersAndStaff = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                        case "News":
                            anime.AdditionalInfoUrls.News = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                        case "Forum":
                            anime.AdditionalInfoUrls.Forum = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                        case "Featured":
                            anime.AdditionalInfoUrls.Featured = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                        case "Clubs":
                            anime.AdditionalInfoUrls.Clubs = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                        case "Pictures":
                            anime.AdditionalInfoUrls.Pictures = child.ChildNodes["a"].Attributes["href"].Value;
                            break;
                    }
                }
            }
        }

        private void GetRelated(HtmlDocument doc, Anime anime)
        {
            var node = doc.DocumentNode.SelectSingleNode("//table[@class='anime_detail_related_anime']").ChildNodes["tr"];
            ParseTd(node, anime);
        }

        private void ParseTd(HtmlNode node, Anime anime)
        {
            switch (node.ChildNodes[0].InnerText.Replace(":",""))
            {
                case "Adaptation":
                    anime.MangaAdaptation.AddRange(MapRelated(node));
                    break;
                case "Prequel":
                    anime.Prequels.AddRange(MapRelated(node));
                    break;
                case "Sequel":
                    anime.Sequels.AddRange(MapRelated(node));
                    break;
                case "Side Story":
                    anime.SideStories.AddRange(MapRelated(node));
                    break;
                case "Parent Story":
                    anime.ParentStory = MapRelated(node).FirstOrDefault();
                    break;
                case "Character Anime":
                    anime.CharacterAnime.AddRange(MapRelated(node));
                    break;
                case "Spin Off":
                    anime.SpinOffs.AddRange(MapRelated(node));
                    break;
                case "Summary":
                    anime.Summaries.AddRange(MapRelated(node));
                    break;
                case "Alternative Versions":
                    anime.AlternativeVersion.AddRange(MapRelated(node));
                    break;
                default:
                    anime.Others.AddRange(MapRelated(node));
                    break;

            }

            if (node.ChildNodes.Count == 3)
            {
                var nextNode = node.ChildNodes[2];
                if (nextNode != null)
                    ParseTd(nextNode, anime);
            }
        }

        private List<Related> MapRelated(HtmlNode node)
        {
            var relatedShows = new List<Related>();

            var subNode = node.ChildNodes[1];
            foreach(var url in subNode.ChildNodes)
            {
                if (url.Name == "a")
                {
                    var related = new Related
                    {
                        Title = url.InnerText,
                        Url = string.Format(CleanMalUrl, url.Attributes["href"].Value)
                    };
                    var parts = related.Url.Split('/');
                    int id;
                    int.TryParse(parts[4], out id);
                    related.Id = id;
                    relatedShows.Add(related);
                }
            }


            return relatedShows;
        }
        #endregion
    }
}