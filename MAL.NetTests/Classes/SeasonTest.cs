using System.Linq;
using FakeItEasy;
using MAL.NetLogic.Classes;
using MAL.NetLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MAL.NetTests.Classes
{
    /// <summary>
    /// Summary description for SeasonTest
    /// </summary>
    [TestClass]
    public class SeasonTest
    {
        [TestMethod]
        public void TestSeason()
        {
            var fakeLog = A.Fake<ILogWriter>();
            var fakeConsole = A.Fake<IConsoleWriter>();
            var fakeFactory = A.Fake<ISeasonFactory>();

            var instance = new SeasonRetriever(fakeLog, fakeConsole, fakeFactory);
            var result = instance.GetSeasonData(2016, "Spring").Result;

            Assert.AreEqual(result.Count, 78);
            var first = result.FirstOrDefault();
            Assert.IsNotNull(first);
            Assert.AreEqual(first.Id, 31737);
            Assert.AreEqual(first.Title, "Gakusen Toshi Asterisk 2nd Season");
        }
    }
}
