﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;
using Serilog;

namespace MAL.NetLogic.Classes
{


    public class AnimeRetriever : IAnimeRetriever
    {
        #region Variables

        private readonly IAnimeFactory _animeFactory;
        private readonly ICharacterFactory _characterFactory;
        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Constructor

        public AnimeRetriever(IAnimeFactory animeFactory, ICharacterFactory characterFactory, IUrlHelper urlHelper)
        {
            _animeFactory = animeFactory;
            _characterFactory = characterFactory;
            _urlHelper = urlHelper;
        }

        #endregion

        public async Task<IAnime> GetAnime(int animeId, string username = "", string password = "")
        {
            var fullTrace = string.Empty;

            var anime = _animeFactory.CreateAnime();

            try
            {
                //Our first task is to retrieve the MAL anime - for now we cheat and grab it from our example data
                var doc = new HtmlDocument();

                var url = string.Format(_urlHelper.MalUrl, animeId);
                HttpClient webClient;

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var handler = new HttpClientHandler {Credentials = new NetworkCredential(username, password)};
                    webClient = new HttpClient(handler);
                }
                else
                {
                    webClient = new HttpClient();
                }
                var data = await webClient.GetStreamAsync(new Uri(url));
                doc.Load(data, Encoding.UTF8);

                //Retrieve the MAL ID
                int aid;
                var idString =
                    doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @name='aid']").Attributes["value"]
                        .Value;
                int.TryParse(idString, out aid);
                anime.Id = aid;

                anime.Title =
                    doc.DocumentNode.SelectSingleNode("//h1").SelectSingleNode("//span[@itemprop='name']").InnerText;

                var synopsis = string.Empty;
                var synopsisNode = doc.DocumentNode.SelectSingleNode("//span[@itemprop='description']");
                if (synopsisNode != null)
                {
                    synopsis = doc.DocumentNode.SelectSingleNode("//span[@itemprop='description']").InnerText;
                }
                else
                {
                    var tableRows = doc.DocumentNode.SelectNodes("//td[@valign='top']");
                    foreach (var row in tableRows)
                    {
                        var header = row.ChildNodes["h2"];
                        if (header != null && header.InnerText.Contains("Synopsis"))
                        {
                            var synopsisData = row.ChildNodes.Where(t => t.Name == "#text").Select(t => t.InnerText).ToList();
                            synopsis = synopsisData[1];
                        }
                    }
                }

                synopsis = synopsis.TrimStart("\r\n".ToCharArray()).Trim();
                synopsis = HttpUtility.HtmlDecode(synopsis);
                anime.Synopsis = synopsis;

                //Retrieve Alternative titles
                var divNodes = doc.DocumentNode.SelectNodes("//div[@class='spaceit_pad']") ??
                               doc.DocumentNode.SelectNodes("//div");


                foreach (var node in divNodes)
                {
                    var lang = node.ChildNodes.Descendants().FirstOrDefault()?.InnerText.Trim(':');

                    switch (lang)
                    {
                        case "Japanese":
                            var jNodes = node.ChildNodes.Where(t => t.Name == "#text");
                            foreach (var jTitle in jNodes.Select(title => title.InnerText.Replace("\r\n", "").Trim()).Where(jTitle => !string.IsNullOrEmpty(jTitle)))
                            {
                                foreach (var innerjTitle in jTitle.Split(',').Select(t => t.Trim()))
                                {
                                    anime.JapaneseTitles.Add(@WebUtility.HtmlDecode(innerjTitle));
                                }
                            }
                            break;
                        case "English":
                            var eNodes = node.ChildNodes.Where(t => t.Name == "#text");
                            foreach (var eTitle in eNodes.Select(title => title.InnerText.Replace("\r\n", "").Trim()).Where(eTitle => !string.IsNullOrEmpty(eTitle)))
                            {
                                foreach (var innereTitle in eTitle.Split(',').Select(t => t.Trim()))
                                {
                                    anime.EnglishTitles.Add(@WebUtility.HtmlDecode(innereTitle));
                                }
                            }
                            break;
                        case "Synonyms":
                            var sNodes = node.ChildNodes.Where(t => t.Name == "#text");
                            foreach (var sTitle in sNodes.Select(title => title.InnerText.Replace("\r\n", "").Trim()).Where(sTitle => !string.IsNullOrEmpty(sTitle)))
                            {
                                foreach (var innersTitle in sTitle.Split(',').Select(t => t.Trim()))
                                {
                                    anime.SynonymousTitles.Add(@WebUtility.HtmlDecode(innersTitle))
                                    ;
                                }
                            }
                            break;
                    }
                }

                var img = doc.DocumentNode.SelectSingleNode("//img[@itemprop='image']")?.Attributes["data-src"]?.Value;
                if (string.IsNullOrEmpty(img))
                {
                    img = doc.DocumentNode.SelectSingleNode("//img[@itemprop='image']")?.Attributes["src"]?.Value;
                }

                //If we cannot find an image check if there is a na_series image
                if (string.IsNullOrEmpty(img))
                {
                    var noImg =
                        doc.DocumentNode.SelectSingleNode(
                            "//img[@src='http://cdn.myanimelist.net/images/qm_50.gif']")?.Attributes["src"].Value;
                    if (string.IsNullOrEmpty(noImg))
                    {
                        noImg = doc.DocumentNode.SelectSingleNode(
                            "//img[@src='http://cdn.myanimelist.net/images/na_series.gif']")?.Attributes["src"].Value;
                    }

                    if (!string.IsNullOrEmpty(noImg))
                    {
                        anime.ImageUrl = noImg;
                        anime.HighResImageUrl = noImg;
                    }
                    else
                    {
                        anime.ImageUrl = null;
                        //throw new Exception("Cannot find the image for this series and there is no na_series.gif");
                    }
                }
                else
                {
                    anime.ImageUrl = img;
                    anime.HighResImageUrl = img.Insert(img.Length - 4, "l");
                }


                foreach (var node in doc.DocumentNode.SelectNodes("//div"))
                {
                    var innerSpan = node.ChildNodes.Descendants().FirstOrDefault()?.InnerText.Trim(':');
                    switch (innerSpan)
                    {
                        case "Type":
                            var tNodes = node.ChildNodes.Where(t => t.Name == "a");
                            foreach (var innerNode in tNodes)
                            {
                                if (innerNode.Attributes["href"].Value.StartsWith("https://myanimelist.net/topanime.php?type="))
                                {
                                    var type = innerNode.InnerText.Replace("\r\n", "").Trim();
                                    anime.Type = type;
                                }
                            }
                            
                            //foreach (var type in tNodes.Select(item => item.InnerText.Replace("\r\n", "").Trim()).Where(type => !string.IsNullOrEmpty(type)))
                            //{
                            //    anime.Type = type;
                            //}
                            break;
                        case "Episodes":
                            var epString = node.ChildNodes["#text"].InnerText.TrimEnd("\n\t".ToCharArray()).Trim();
                            if (epString.ToLower() == "unknown")
                            {
                                anime.Episodes = -1;
                            }
                            else
                            {
                                int eps;
                                int.TryParse(epString, out eps);
                                if (eps == 0)
                                {
                                    epString = node.ChildNodes[2].InnerText.Replace("\r\n", "").Trim();
                                    int.TryParse(epString, out eps);
                                    anime.Episodes = eps;
                                }

                                if (eps == 0)
                                    anime.Episodes = null;
                                else
                                    anime.Episodes = eps;
                            }
                            break;
                        case "Status":
                            var stNode = node.ChildNodes.Where(t => t.Name == "#text");
                            foreach (
                                var stat in
                                    stNode.Select(t => t.InnerText.Replace("\r\n", "").Trim())
                                        .Where(type => !string.IsNullOrEmpty(type)))
                            {
                                anime.Status = stat;
                            }
                            break;
                        case "Aired":
                            var dateNodes = node.ChildNodes.Where(t => t.Name == "#text");
                            foreach (var item in dateNodes)
                            {
                                var dateString = item.InnerText.Replace("\r\n", "").Trim();
                                if (!string.IsNullOrEmpty(dateString))
                                {
                                    var dates = Regex.Split(dateString, " to ");
                                    var startDate = DateTime.MinValue;
                                    var endDate = DateTime.MinValue;
                                    if (dates.Any())
                                        DateTime.TryParse(dates[0], out startDate);
                                    if (dates.Count() > 1)
                                        DateTime.TryParse(dates[1], out endDate);
                                    anime.StartDate = startDate;
                                    anime.EndDate = endDate;
                                }
                            }     
                            break;
                        case "Rating":
                            var txt = node.InnerText.Replace("\r\n", "");
                            var cleanText = Regex.Split(txt, "                                    ").Last().Trim();
                            cleanText = cleanText.Replace("Rating:", "").Replace("Rating:", "").Trim(Environment.NewLine.ToCharArray()).Trim();
                            anime.Classification = cleanText;
                            break;
                        case "Ranked":
                            var rankNodes = node.ChildNodes.Where(t => t.Name == "#text");
                            foreach (var randNode in rankNodes)
                            {
                                var rankString = randNode.InnerText.Replace("\r\n", "").Trim().TrimStart('#');
                                if (!string.IsNullOrEmpty(rankString))
                                {
                                    int number;
                                    int.TryParse(rankString, out number);
                                    anime.Rank = number;
                                }
                            }
                            break;
                        case "Popularity":
                            var popNodes = node.ChildNodes.Where(t => t.Name == "#text");
                            foreach(var popItem in popNodes)
                            {
                                var pString = popItem.InnerText.Trim().TrimStart('#');
                                if (!string.IsNullOrEmpty(pString))
                                {
                                    int pNum;
                                    int.TryParse(pString, out pNum);
                                    anime.Popularity = pNum;
                                }
                            }
                            break;
                        case "Score":
                            string scoreString;
                            var scoreNode = node.SelectNodes("//span[@itemprop='ratingValue']");
                            if (scoreNode != null && scoreNode.Count >= 1)
                            {
                                scoreString = scoreNode[0].InnerText;
                            }
                            else
                            {
                                var sNode = node.ChildNodes["#text"].InnerText;
                                scoreString = sNode;
                            }
                            double scoreVal;
                            double.TryParse(scoreString, NumberStyles.Any, CultureInfo.InvariantCulture, out scoreVal);
                            anime.MemberScore = scoreVal;
                            break;
                        case "Members":
                            var memberNodes = node.ChildNodes.Where(t => t.Name == "#text");
                            foreach (var memberNode in memberNodes)
                            {
                                var memberString = memberNode.InnerText.Replace("\r\n", "").Trim().Replace(",", "");
                                if (!string.IsNullOrEmpty(memberString))
                                {
                                    int mVal;
                                    int.TryParse(memberString, out mVal);
                                    anime.MemberCount = mVal;
                                }
                            }                         
                            break;
                        case "Favorites":
                            var favNodes = node.ChildNodes.Where(t => t.Name == "#text");
                            foreach (var favNode in favNodes)
                            {
                                var favString = favNode.InnerText.Replace("\r\n", "").Trim();
                                if (!string.IsNullOrEmpty(favString))
                                {
                                    int fVal;
                                    int.TryParse(favString, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal);
                                    anime.FavoriteCount = fVal;
                                }
                            }
                            break;
                        case "Genres":
                            var genreNodes = node.ChildNodes.Where(t => t.Name == "a");
                            if (genreNodes == null) break;
                            foreach (var g in genreNodes)
                            {
                                anime.Genres.Add(g.InnerText);
                            }
                            break;
                    }
                }

                //var tagNodes = doc.DocumentNode.SelectNodes("//div[@class='tags-inner']");

                //if (tagNodes != null)
                //{

                //    foreach (var tagNode in tagNodes)
                //    {
                //        foreach (var tag in tagNode.ChildNodes.Nodes())
                //        {
                //            if (tag.OriginalName == "#text" && !anime.Tags.Contains(tag.InnerText))
                //                anime.Tags.Add(tag.InnerText);
                //        }
                //    }
                //}

                GetInfoUrls(doc, anime);
                GetRelated(doc, anime);
                await GetCharacterAndSeiyuuInformation(anime, username, password);

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    foreach (var statusNode in doc.DocumentNode.SelectSingleNode("//select[@name='myinfo_status']").ChildNodes)
                    {
                        if (statusNode.GetAttributeValue("selected", "") != "selected") continue;
                        var status = statusNode.NextSibling.InnerText;
                        anime.UserWatchedStatus = status;
                    }

                    var epNode =
                        doc.DocumentNode.SelectSingleNode("//input[@type='text' and @name='myinfo_watchedeps']")
                            .Attributes["value"].Value;
                    int myScore;
                    int.TryParse(epNode, out myScore);
                    anime.UserWatchedEpisodes = myScore;

                    foreach (
                        var scoreNodes in doc.DocumentNode.SelectSingleNode("//select[@name='myinfo_score']").ChildNodes
                        )
                    {
                        if (scoreNodes.GetAttributeValue("selected", "") != "selected") continue;
                        var epWatched = scoreNodes.Attributes["value"].Value;
                        int usrScore;
                        int.TryParse(epWatched, out usrScore);
                        anime.UserScore = usrScore;
                    }
                }
            }
            catch (Exception ex)
            {
                anime.ErrorOccured = true;
                anime.ErrorMessage = ex.Message;
                Log.Error(ex, "Error occured while trying to retrieve {AnimeId}", animeId);
            }

            return anime;
        }

        #region Private Methods

        private async Task GetCharacterAndSeiyuuInformation(IAnime anime, string username, string password)
        {
            try
            {
                //Our first task is to retrieve the MAL anime - for now we cheat and grab it from our example data
                var doc = new HtmlDocument();

                var url = anime.AdditionalInfoUrls.CharactersAndStaff;
                HttpClient webClient;

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    var handler = new HttpClientHandler {Credentials = new NetworkCredential(username, password)};
                    webClient = new HttpClient(handler);
                }
                else
                {
                    webClient = new HttpClient();
                }
                var data = await webClient.GetStreamAsync(new Uri(url));
                doc.Load(data);

                var tableNodes = doc.DocumentNode.SelectNodes("//table");
                foreach (var table in tableNodes)
                {
                    var rows = table.ChildNodes.Where(t => t.Name == "tr");
                    //var rows = table.SelectNodes("//tr");
                    if (rows != null)
                    {
                        foreach (var row in rows)
                        {
                            var columns = row.ChildNodes.Where(t => t.Name == "td").ToList();
                            if (columns.Count == 3)
                            {
                                var tmpChar = _characterFactory.CreateCharacter();
                                var charPic = columns[0].ChildNodes["div"].ChildNodes["a"].ChildNodes["img"].Attributes["data-src"]?.Value;
                                if (string.IsNullOrEmpty(charPic))
                                {
                                    charPic = columns[0].ChildNodes["div"].ChildNodes["a"].ChildNodes["img"].Attributes["src"]?.Value;
                                }
                                tmpChar.CharacterPicture = charPic;


                                tmpChar.CharacterName = columns[1].ChildNodes["a"].InnerText;
                                tmpChar.CharacterUrl = columns[1].ChildNodes["a"].Attributes["href"].Value;
                                tmpChar.CharacterType = columns[1].ChildNodes["div"].InnerText;

                                var vaDetail = columns[2].ChildNodes["table"]?.ChildNodes.Where(t => t.Name == "tr").ToList();
                                if(vaDetail == null) continue;
                                foreach (var detail in vaDetail)
                                {
                                    var tmpSeiyuu = _characterFactory.CreateSeiyuu();
                                    tmpSeiyuu.Language = detail.ChildNodes["td"].ChildNodes["small"].InnerText;
                                    tmpSeiyuu.Name = detail.ChildNodes["td"].ChildNodes["a"].InnerText;
                                    tmpSeiyuu.Url = detail.ChildNodes["td"].ChildNodes["a"].Attributes["href"].Value;
                                    var tmpImage = detail.ChildNodes[3].ChildNodes["div"].ChildNodes["a"].ChildNodes["img"].Attributes["data-src"]?.Value;
                                    if (string.IsNullOrEmpty(tmpImage))
                                    {
                                        tmpImage = detail.ChildNodes[3].ChildNodes["div"].ChildNodes["a"].ChildNodes["img"].Attributes["src"].Value;
                                    }
                                    tmpSeiyuu.PictureUrl = tmpImage;
                                    tmpChar.Seiyuu.Add(tmpSeiyuu);
                                }
                                if(anime.CharacterInformation.Count(t => t.CharacterUrl == tmpChar.CharacterUrl) == 0)
                                    anime.CharacterInformation.Add(tmpChar);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occured while trying to retrieve character and staff data");
            }

        }

        private void GetInfoUrls(HtmlDocument doc, IAnime anime)
        {
            foreach (var listItem in doc.DocumentNode.SelectNodes("//div[@id='horiznav_nav']"))
            {
                foreach (var child in listItem.ChildNodes["ul"].ChildNodes)
                {
                    var item = child.ChildNodes["a"];
                    if (item == null) continue;
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
                        case "Characters & Staff":
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

        private void GetRelated(HtmlDocument doc, IAnime anime)
        {
            var relatedNodes = doc.DocumentNode.SelectSingleNode("//table[@class='anime_detail_related_anime']");

            if (relatedNodes != null)
            {
                foreach (var node in relatedNodes.ChildNodes)
                {
                    ParseTd(node, anime);
                }
            }
        }

        private void ParseTd(HtmlNode node, IAnime anime)
        {
            switch (node.ChildNodes[0].InnerText.Replace(":", "").ToLower())
            {
                case "adaptation":
                    anime.MangaAdaptation.AddRange(MapRelated(node));
                    break;
                case "prequel":
                    anime.Prequels.AddRange(MapRelated(node));
                    break;
                case "sequel":
                    anime.Sequels.AddRange(MapRelated(node));
                    break;
                case "side story":
                    anime.SideStories.AddRange(MapRelated(node));
                    break;
                case "parent story":
                    anime.ParentStory = MapRelated(node).FirstOrDefault();
                    break;
                case "character":
                    anime.CharacterAnime.AddRange(MapRelated(node));
                    break;
                case "spin-off":
                    anime.SpinOffs.AddRange(MapRelated(node));
                    break;
                case "summary":
                    anime.Summaries.AddRange(MapRelated(node));
                    break;
                case "alternative version":
                    anime.AlternativeVersion.AddRange(MapRelated(node));
                    break;
                case "alternative setting":
                    anime.AlternativeSetting.AddRange(MapRelated(node));
                    break;
                case "full story":
                    anime.FullStories.AddRange(MapRelated(node));
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
            foreach (var url in subNode.ChildNodes)
            {
                if (url.Name == "a")
                {
                    var related = new Related
                    {
                        Title = url.InnerText,
                        Url = string.Format(_urlHelper.CleanMalUrl, url.Attributes["href"].Value)
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