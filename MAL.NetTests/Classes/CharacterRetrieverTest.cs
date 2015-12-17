using System;
using FakeItEasy;
using MAL.NetLogic.Classes;
using MAL.NetLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MAL.NetTests.Classes
{
    [TestClass]
    public class CharacterRetrieverTest
    {
        [TestMethod]
        public void TestAsunaYuuki()
        {
            var startDate = new DateTime(2012, 7, 8);
            var endDate = new DateTime(2012, 12, 23);

            var fakeFactory = A.Fake<IAnimeFactory>();
            var fakeAnime = A.Fake<IAnime>();
            var fakeJson = A.Fake<IAnimeOriginalJson>();
            var fakeLog = A.Fake<ILogWriter>();
            var fakeWriter = A.Fake<IConsoleWriter>();
            var fakeCharFactory = A.Fake<ICharacterFactory>();

            A.CallTo(() => fakeFactory.CreateAnime()).Returns(fakeAnime);
            A.CallTo(() => fakeFactory.CreateJsonAnime()).Returns(fakeJson);

            var instance = new CharacterRetriever(fakeLog, fakeWriter, fakeCharFactory);
            var tResult = instance.GetCharacter(36828);
        }
    }
}
