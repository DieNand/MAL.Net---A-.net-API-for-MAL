using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MAL.NetLogic.Interfaces;
using Serilog;

namespace MAL.NetLogic.Classes
{
    public class CharacterRetriever
    {
        #region Variables

        private readonly ICharacterFactory _characterFactory;
        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Constructor

        public CharacterRetriever(ICharacterFactory characterFactory, IUrlHelper urlHelper)
        {
            _characterFactory = characterFactory;
            _urlHelper = urlHelper;
        }

        #endregion

        #region Public Methods

        public async Task<ICharacter> GetCharacter(int characterId)
        {
            var fullTrace = string.Empty;
            var url = string.Empty;

            var character = _characterFactory.CreateFullCharacter();

            try
            {
                //Our first task is to retrieve the MAL anime - for now we cheat and grab it from our example data
                var doc = new HtmlDocument();

                url = string.Format(_urlHelper.CharacterUrl, characterId);

                var webClient = new HttpClient();
                var htmlData = await webClient.GetStreamAsync(new Uri(url));
                doc.Load(htmlData);

                character.Id = characterId;
                character.Url = url;
                character.Name = doc.DocumentNode.SelectNodes("//div[@class='normal_header']").ToList()[2].InnerText;
                var leftPanelTable = doc.DocumentNode.SelectNodes("//table")[0];
                var rows = leftPanelTable.ChildNodes["tr"];
                character.ImageUrl = rows.ChildNodes["td"].ChildNodes["div"].ChildNodes["img"].Attributes["data-src"].Value;

                var data = rows.ChildNodes["td"].ChildNodes.FirstOrDefault(t => t.InnerText.Contains("Member Favorites"));
                if (data != null)
                {
                    var cleanData = data.InnerText.Replace("\r\n", "").Replace("Member Favorites:", "").Trim();
                    int memberCount;
                    int.TryParse(cleanData, out memberCount);
                    character.FavoriteCount = memberCount;
                }

                var bioData = rows.ChildNodes[3].ChildNodes.Where(t => t.Name == "#text");
                //We also need to pull in any spoiler info
                var spoilerData = doc.DocumentNode.SelectNodes("//div[@class='spoiler']");

                var sBuilder = new StringBuilder();
                foreach (var item in bioData)
                {
                    var tmpText = item.InnerText.Replace("\r\n", "").Trim();
                    sBuilder.Append($"{tmpText} ");
                }
                if (spoilerData.Count > 0)
                {
                    sBuilder.Append(" <SPOILER>");
                    foreach (var spoiler in spoilerData)
                    {
                        sBuilder.Append($"{spoiler.InnerText.Replace("<!--spoiler-->", "")} ");
                    }
                    sBuilder.Append("</SPOILER>");
                }
                character.Biography = sBuilder.ToString().Trim();

                await GetAnimeography(character, doc);

            }
            catch (Exception ex)
            {
                character.ErrorOccured = true;
                character.ErrorMessage = ex.Message;
                Log.Error(ex, "Error occured while retrieving {Character Id}", character.Id);
            }

            return character;
        }

        #endregion

        #region Private Methods

        private async Task GetAnimeography(ICharacter character, HtmlDocument doc)
        {
            var ographyTabes =
                doc.DocumentNode.SelectNodes("//table")[0].ChildNodes["tr"].ChildNodes["td"].ChildNodes.Where(
                    t => t.Name == "table");
            //First table contains animeography, second the mangaography
        }
        
        #endregion
    }
}