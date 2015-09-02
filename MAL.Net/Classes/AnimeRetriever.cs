using System;
using System.IO;
using System.Linq;
using System.Reflection;
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