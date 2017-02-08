using System;
using System.IO;
using FluentAssertions;
using HttpMock;
using MAL.NetLogic.Classes;
using MAL.NetLogic.Interfaces;
using MAL.NetLogic.Objects;
using Moq;
using NUnit.Framework;

namespace MAL.NetTests.Classes
{
    public class RelationshipTest
    {
        [TestCase(6922, 0,1,0,0,0,0,0,3,0,0,false)]
        [TestCase(6213, 1,0,1,2,1,1,0,0,0,0,true)]
        [TestCase(22273,0,0,1,1,0,0,1,0,1,0,false)]
        [TestCase(7559, 0,0,0,0,0,0,0,0,0,1,false)]
        public void EnsureRelationshipsAreCorrect(int malId, int adaptations, int prequel, int sequel, int sideStory,
            int charAnime, int spinOff, int summary, int altVersion, int altSetting, int fullStory, bool hasParent)
        {
            // arrange
            var animeDummy = new Anime();
            var jsonDummy = new Mock<IAnimeOriginalJson>();
            var fixture = new RetrieverFixture("http://localhost:8084");

            fixture.SetupHttpMock(malId);
            fixture.AnimeFactoryMock.Setup(t => t.CreateAnime()).Returns(animeDummy);
            fixture.AnimeFactoryMock.Setup(t => t.CreateJsonAnime()).Returns(jsonDummy.Object);

            var sut = fixture.Instance;

            // act
            var result = sut.GetAnime(malId).Result;

            // assert
            result.MangaAdaptation.Count.Should().Be(adaptations);
            result.Prequels.Count.Should().Be(prequel);
            result.Sequels.Count.Should().Be(sequel);
            result.SideStories.Count.Should().Be(sideStory);
            result.CharacterAnime.Count.Should().Be(charAnime);
            result.SpinOffs.Count.Should().Be(spinOff);
            result.Summaries.Count.Should().Be(summary);
            result.AlternativeVersion.Count.Should().Be(altVersion);
            result.AlternativeSetting.Count.Should().Be(altSetting);
            result.FullStories.Count.Should().Be(fullStory);
            if (hasParent)
            {
                result.ParentStory.Should().NotBeNull();
            }
            else
            {
                result.ParentStory.Should().BeNull();
            }
        }

        public class RetrieverFixture
        {
            public IHttpServer HttpServerMock { get; }
            public AnimeRetriever Instance { get; }

            public Mock<IAnimeFactory> AnimeFactoryMock { get; } = new Mock<IAnimeFactory>();
            public Mock<ICharacterFactory> CharacterFactoryMock { get; } = new Mock<ICharacterFactory>();
            public Mock<IUrlHelper> UrlHelperMock { get; } = new Mock<IUrlHelper>();

            public RetrieverFixture(string mockUrl)
            {
                HttpServerMock = HttpMockRepository.At(mockUrl);
                UrlHelperMock.Setup(t => t.MalUrl).Returns($"{mockUrl}/anime/{{0}}");
                UrlHelperMock.Setup(t => t.CleanMalUrl).Returns("http://myanimelist.net{0}");
                Instance = new AnimeRetriever(AnimeFactoryMock.Object, CharacterFactoryMock.Object, UrlHelperMock.Object);
            }

            public void SetupHttpMock(int idToMock)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory;
                var file = Path.Combine(path, "AnimeExamples", $"{idToMock}.html");
                var content = File.ReadAllText(file);
                HttpServerMock
                    .Stub(x => x.Get($"/anime/{idToMock}"))
                    .Return(content)
                    .OK();
            }
        }

    }
}