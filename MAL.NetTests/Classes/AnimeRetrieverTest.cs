using MAL.Net.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MAL.NetTests.Classes
{
    [TestClass]
    public class AnimeRetrieverTest
    {
        [TestMethod]
        public void TestAnimeRetrieval()
        {
            var instance = new AnimeRetriever();
            var result = instance.GetAnime(28999);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.ErrorOccured);
            Assert.IsTrue(string.IsNullOrEmpty(result.ErrorMessage));
        }
    }
}
