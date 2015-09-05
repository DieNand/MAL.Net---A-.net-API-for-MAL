using System;
using System.Linq;
using MAL.Net.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MAL.NetTests.Classes
{
    [TestClass]
    public class AnimeRetrieverTest
    {
        [TestMethod]
        public void TestSwordArtRetrieval()
        {
            var startDate = new DateTime(2012, 7, 8);
            var endDate = new DateTime(2012, 12, 23);

            var instance = new AnimeRetriever();
            var result = instance.GetAnime(11757);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.ErrorOccured);
            Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));

            Assert.AreEqual(11757, result.Id);
            Assert.AreEqual("Sword Art Online", result.Title);
            Assert.IsFalse(string.IsNullOrEmpty(result.Synopsis));
            Assert.AreEqual(1, result.JapaneseTitles.Count);
            Assert.AreEqual("ソードアート・オンライン", result.JapaneseTitles.First());
            Assert.AreEqual(1, result.EnglishTitles.Count);
            Assert.AreEqual("Sword Art Online", result.EnglishTitles.First());
            Assert.AreEqual(2, result.SynonymousTitles.Count);
            Assert.AreEqual("S.A.O", result.SynonymousTitles[0]);
            Assert.AreEqual("SAO", result.SynonymousTitles[1]);

            Assert.AreEqual("TV", result.Type);
            Assert.AreEqual(25, result.Episodes);
            Assert.AreEqual("PG-13 - Teens 13 or older", result.Classification);
            Assert.AreEqual("Finished Airing", result.Status);
            Assert.AreEqual(startDate, result.StartDate);
            Assert.AreEqual(endDate, result.EndDate);

            Assert.AreEqual(2, result.Popularity);
            Assert.AreEqual(464, result.Rank);
            Assert.AreEqual("http://cdn.myanimelist.net/images/anime/11/39717.jpg", result.ImageUrl);
            Assert.AreEqual("http://cdn.myanimelist.net/images/anime/11/39717l.jpg", result.HighResImageUrl);

            Assert.AreEqual(8.05, result.MemberScore);
            Assert.AreEqual(580338, result.MemberCount);
            Assert.AreEqual(31613, result.FavoriteCount);

            Assert.AreEqual(0, result.UserScore);
            Assert.AreEqual(0, result.UserWatchedEpisodes);
            Assert.IsTrue(string.IsNullOrEmpty(result.UserWatchedStatus));

            Assert.AreEqual(5, result.Genres.Count);
            Assert.AreEqual(7, result.Tags.Count);

            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/episode", result.AdditionalInfoUrls.Episodes);
            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/reviews", result.AdditionalInfoUrls.Reviews);
            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/userrecs", result.AdditionalInfoUrls.Recommendation);
            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/stats", result.AdditionalInfoUrls.Stats);
            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/characters", result.AdditionalInfoUrls.CharactersAndStaff);
            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/news", result.AdditionalInfoUrls.News);
            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/forum", result.AdditionalInfoUrls.Forum);
            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/featured", result.AdditionalInfoUrls.Featured);
            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/clubs", result.AdditionalInfoUrls.Clubs);
            Assert.AreEqual("http://myanimelist.net/anime/11757/Sword_Art_Online/pics", result.AdditionalInfoUrls.Pictures);

            Assert.AreEqual(2, result.MangaAdaptation.Count);
            Assert.AreEqual(0, result.Prequels.Count);
            Assert.AreEqual(1, result.Sequels.Count);
            Assert.AreEqual(0, result.SideStories.Count);
            Assert.IsNull(result.ParentStory);
            Assert.AreEqual(0, result.CharacterAnime.Count);
            Assert.AreEqual(0, result.SpinOffs.Count);
            Assert.AreEqual(0, result.Summaries.Count);
            Assert.AreEqual(0, result.AlternativeVersion.Count);
            Assert.AreEqual(1, result.Others.Count);

            var sequelInfo = result.Sequels.First();
            Assert.AreEqual(20021, sequelInfo.Id);
            Assert.AreEqual("Sword Art Online: Extra Edition", sequelInfo.Title);
            Assert.AreEqual("http://myanimelist.net/anime/20021/Sword_Art_Online:_Extra_Edition", sequelInfo.Url);
        }
    }
}
