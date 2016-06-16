using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MAL.NetLogic.Interfaces;
using Serilog;

namespace MAL.NetLogic.Classes
{
    public class SeasonRetriever : ISeasonRetriever
    {
        #region Variables

        #endregion
        
        private readonly ISeasonFactory _seasonFactory;
        private readonly ISeasonLookup _seasonLookup;
        private readonly IUrlHelper _urlHelper;

        #region Constructor

        public SeasonRetriever(ISeasonFactory seasonFactory, ISeasonLookup seasonLookup, IUrlHelper urlHelper)
        {
            _seasonFactory = seasonFactory;
            _seasonLookup = seasonLookup;
            _urlHelper = urlHelper;
        }

        #endregion

        #region Public Methods

        public async Task<List<ISeasonData>> GetSeasonData(int year, string season)
        {
            var seasonList = new List<ISeasonData>();
            try
            {
                var doc = new HtmlDocument();
                var uri = string.Format(_urlHelper.SeasonUrl, year, season.ToLower(CultureInfo.InvariantCulture));
                Log.Information("Quering {Uri}", uri);
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
                Log.Error(ex, "An error occured while retrieving season data for {year} - {season}", year, season);
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
                Log.Information("Retrieving season data {Round} which contains {year} - {season}", r, year, currentSeason);
                var tmpData = await GetSeasonData(year, currentSeason);
                seasonData.AddRange(tmpData);
                Log.Information("Retrieved season data {Round} which contains {year} - {season}", r, year, currentSeason);

                //Get info for the next seas
                year = _seasonLookup.NextSeasonYear(currentSeason, year);
                currentSeason = _seasonLookup.GetNextSeason(currentSeason);
            }

            return seasonData;
        }

#endregion
    }
}