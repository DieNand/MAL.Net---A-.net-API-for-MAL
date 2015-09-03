using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MAL.Net.Objects;

namespace MAL.Net.Classes
{
    public class AnimeRetriever
    {
        #region Variables

        private const string MalUrl = @"http://myanimelist.net/anime/{0}";
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

                var img = doc.DocumentNode.SelectSingleNode("//img[@itemprop='image']").Attributes["src"].Value;
                anime.ImageUrl = img;
                anime.HighResImageUrl = img.Insert(img.Length - 4, "l");

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
                            double.TryParse(scoreString, out scoreVal);
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
                            int.TryParse(favString, out fVal);
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

            }
            catch (Exception ex)
            {
                anime.ErrorOccured = true;
                anime.ErrorMessage = ex.Message;
            }

            return anime;
        }
    }
}