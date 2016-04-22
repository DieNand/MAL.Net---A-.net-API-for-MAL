using System;
using System.IO;
using System.Reflection;
using FakeItEasy;
using HttpMock;
using MAL.NetLogic.Classes;
using MAL.NetLogic.Helpers;
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
            var charId = 36828;
            //Mock the HttpClient - This allows us to control the response
            var httpMock = HttpMockRepository.At("http://localhost:8081");
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(path, "AnimeExamples", $"{charId}.html");
            var content = File.ReadAllText(file);
            httpMock.Stub(x => x.Get($"/character/{charId}")).Return(content).OK();

            var startDate = new DateTime(2012, 7, 8);
            var endDate = new DateTime(2012, 12, 23);

            var fakeFactory = A.Fake<IAnimeFactory>();
            var fakeAnime = A.Fake<IAnime>();
            var fakeJson = A.Fake<IAnimeOriginalJson>();
            var fakeLog = A.Fake<ILogWriter>();
            var fakeWriter = A.Fake<IConsoleWriter>();
            var fakeCharFactory = A.Fake<ICharacterFactory>();
            var fakeUrlHelper = A.Fake<IUrlHelper>();

            A.CallTo(() => fakeFactory.CreateAnime()).Returns(fakeAnime);
            A.CallTo(() => fakeFactory.CreateJsonAnime()).Returns(fakeJson);
            A.CallTo(() => fakeUrlHelper.CharacterUrl).Returns(@"http://localhost:8081/character/{0}");

            var instance = new CharacterRetriever(fakeLog, fakeWriter, fakeCharFactory, fakeUrlHelper);
            var tResult = instance.GetCharacter(36828);
        }
    }
}
