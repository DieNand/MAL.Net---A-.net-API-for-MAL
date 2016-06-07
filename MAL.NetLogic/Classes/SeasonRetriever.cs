using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MAL.NetLogic.Helpers;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Classes
{
    public class SeasonRetriever : ISeasonRetriever
    {
        #region Variables

        #endregion
        

        private readonly ILogWriter _logWriter;
        private readonly IConsoleWriter _consoleWriter;
        private readonly ISeasonFactory _seasonFactory;
        private readonly ISeasonLookup _seasonLookup;
        private readonly IUrlHelper _urlHelper;

        #region Constructor

        public SeasonRetriever(ILogWriter logWriter, IConsoleWriter consoleWriter, ISeasonFactory seasonFactory, ISeasonLookup seasonLookup, IUrlHelper urlHelper)
        {
            _logWriter = logWriter;
            _consoleWriter = consoleWriter;
            _seasonFactory = seasonFactory;
            _seasonLookup = seasonLookup;
            _urlHelper = urlHelper;
        }

        #endregion

        #region Public Methods

        public async Task<List<ISeasonData>> GetSeasonData(int year, string season)
        {
            var seasonList = new List<ISeasonData>();
            var errorOccured = false;
            var fullTrace = string.Empty;

            try
            {
                var doc = new HtmlDocument();
                var uri = string.Format(_urlHelper.SeasonUrl, year, season.ToLower(CultureInfo.InvariantCulture));
                _consoleWriter.WriteAsLineEnd($"{new DateTime()} [Season] Querying - {uri}", ConsoleColor.DarkYellow);
                var webClient = new HttpClient();
                var data = await webClient.GetStreamAsync(new Uri(uri));
                doc.Load(data, Encoding.UTF8);

                var links = doc.DocumentNode.SelectNodes("//a[@class='link-title']");
                foreach (var link in links)
                {
                    var url = link.Attributes["href"].Value;
                    var splitUrl = url.Split('/');
                    var id = splitUrl[4];
                    int intId;
                    int.TryParse(id, out intId);

                    var title = link.InnerHtml;

                    var tmpData = _seasonFactory.CreateSeasonData();
                    tmpData.Id = intId;
                    tmpData.Title = title;
                    seasonList.Add(tmpData);
                }


            }
            catch (Exception ex)
            {
                _consoleWriter.WriteAsLineEnd($"Error occured:\r\n{ex}", ConsoleColor.Red);
                fullTrace = ex.ToString();
                errorOccured = true;
            }

            if (errorOccured)
            {
                _logWriter.WriteLogData($"Error occured retrieving season data for {year} - {season}\r\n{fullTrace}");
            }
            return seasonList;
        }

        public async Task<List<ISeasonData>> RetrieveCurrentSeason()
        {
            var seasonData = new List<ISeasonData>();
            var currentSeason = _seasonLookup.CalculateCurrentSeason(DateTime.Now);
            var year = DateTime.Now.Year;
            if (currentSeason == "Winter" && DateTime.Now.Month == 12)
            {
                //For some reason MAL classifies winter 2015 as winter 2016 so adjust for this fact
                year++;
            }

            for (var r = 0; r < 3; r++)
            {
                _consoleWriter.WriteAsLineEnd($"{DateTime.Now} - Retrieving Season data for season {r} - {currentSeason}{year}", ConsoleColor.Gray);
                var tmpData = await GetSeasonData(year, currentSeason);
                seasonData.AddRange(tmpData);
                _consoleWriter.WriteAsLineEnd($"{DateTime.Now} - Retrieved Season data for season {r} - {currentSeason}{year}. {tmpData.Count} item.", ConsoleColor.Gray);

                //Get info for the next seas
                year = _seasonLookup.NextSeasonYear(currentSeason, year);
                currentSeason = _seasonLookup.GetNextSeason(currentSeason);
            }

            return seasonData;
        }

#endregion
    }
}