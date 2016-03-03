using System;
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
            var fakeSeasonLookup = A.Fake<ISeasonLookup>();

            var instance = new SeasonRetriever(fakeLog, fakeConsole, fakeFactory, fakeSeasonLookup);
            var result = instance.GetSeasonData(2016, "Spring").Result;

            Assert.AreEqual(result.Count, 78);
            var first = result.FirstOrDefault();
            Assert.IsNotNull(first);
            Assert.AreEqual(first.Id, 31737);
            Assert.AreEqual(first.Title, "Gakusen Toshi Asterisk 2nd Season");
        }

        [TestMethod]
        public void TestCurrentSeason()
        {
            var fakeLog = A.Fake<ILogWriter>();
            var fakeConsole = A.Fake<IConsoleWriter>();
            var fakeFactory = A.Fake<ISeasonFactory>();
            var fakeSeasonLookup = A.Fake<ISeasonLookup>();

            A.CallTo(() => fakeSeasonLookup.CalculateCurrentSeason(A<DateTime>._)).Returns("Winter");
            A.CallTo(() => fakeSeasonLookup.GetNextSeason("Winter")).Returns("Spring");
            A.CallTo(() => fakeSeasonLookup.GetNextSeason("Spring")).Returns("Summer");
            A.CallTo(() => fakeSeasonLookup.NextSeasonYear("Winter", 2016)).Returns(2016);
            A.CallTo(() => fakeSeasonLookup.NextSeasonYear("Spring", 2016)).Returns(2016);

            var instance = new SeasonRetriever(fakeLog, fakeConsole, fakeFactory, fakeSeasonLookup);
            var result = instance.RetrieveCurrentSeason().Result;

            Assert.AreEqual(result.Count, 299);


        }

    }
}
