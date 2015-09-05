using System;
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

                //Retrieve Alternative titles
                foreach (var node in doc.DocumentNode.SelectNodes("//div[@class='spaceit_pad']"))
                {
                    var lang = node.ChildNodes.Descendants().FirstOrDefault()?.InnerText.Trim(':');

                    switch (lang)
                    {
                        case "Japanese":
                            var jTitle = node.ChildNodes["#text"].InnerText;
                            anime.JapaneseTitles.AddRange(jTitle.Split(','));
                            break;
                        case "English":
                            var eTitle = node.ChildNodes["#text"].InnerText;
                            anime.EnglishTitles.AddRange(eTitle.Split(','));
                            break;
                        case "Synonyms":
                            var sTitle = node.ChildNodes["#text"].InnerText;
                            anime.SynonymousTitles.AddRange(sTitle.Split(','));
                            break;
                    }
                }

                foreach (var node in doc.DocumentNode.SelectNodes("//div"))
                {
                    var innerSpan = node.ChildNodes.Descendants().FirstOrDefault()?.InnerText.Trim(':');
                    switch (innerSpan)
                    {
                        case "Type":
                            anime.Type = node.ChildNodes["#text"].InnerText;
                            break;
                        case "Episodes":
                            var epString = node.ChildNodes["#text"].InnerText;
                            int eps;
                            if (!int.TryParse(epString, out eps))
                                anime.Episodes = null;
                            else
                                anime.Episodes = eps;
                            break;
                        case "Status":
                            anime.Status = node.ChildNodes["#text"].InnerText;
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
                    anime.MangaAdaptation.Add(MapRelated(node));
                    break;
                case "Prequel":
                    anime.Prequels.Add(MapRelated(node));
                    break;
                case "Sequel":
                    anime.Sequels.Add(MapRelated(node));
                    break;
                case "Side Story":
                    anime.SideStories.Add(MapRelated(node));
                    break;
                case "Parent Story":
                    anime.ParentStory = MapRelated(node);
                    break;
                case "Character Anime":
                    anime.CharacterAnime.Add(MapRelated(node));
                    break;
                case "Spin Off":
                    anime.SpinOffs.Add(MapRelated(node));
                    break;
                case "Summary":
                    anime.Summaries.Add(MapRelated(node));
                    break;
                case "Alternative Versions":
                    anime.AlternativeVersion.Add(MapRelated(node));
                    break;
                default:
                    anime.Others.Add(MapRelated(node));
                    break;

            }
            if (node.ChildNodes.Count == 3)
            {
                var nextNode = node.ChildNodes[2];
                if (nextNode != null)
                    ParseTd(nextNode, anime);
            }
        }

        private Related MapRelated(HtmlNode node)
        {
            var subNode = node.ChildNodes[1];
            var url = subNode.ChildNodes["a"].Attributes["href"].Value;
            var related = new Related
            {
                Title = subNode.InnerText,
                Url = string.Format(CleanMalUrl, url)
            };
            var parts = url.Split('/');
            int id;
            int.TryParse(parts[2], out id);
            related.Id = id;
            return related;
        }
        #endregion
    }
}