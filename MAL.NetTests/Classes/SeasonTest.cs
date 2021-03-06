﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using FakeItEasy;
using HttpMock;
using MAL.NetLogic.Classes;
using MAL.NetLogic.Interfaces;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace MAL.NetTests.Classes
{
    /// <summary>
    /// Summary description for SeasonTest
    /// </summary>
    [TestFixture]
    public class SeasonTest
    {
        [Test]
        public void TestSeason()
        {
            const int year = 2016;
            const string season = "Spring";

            var httpMock = HttpMockRepository.At("http://localhost:8082");
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var file = Path.Combine(path, "AnimeExamples", $"{year}{season}.html");
            var content = File.ReadAllText(file);
            httpMock.Stub(x => x.Get($"/anime/season/{year}/{season.ToLower(CultureInfo.InvariantCulture)}")).Return(content).OK();

            var fakeFactory = A.Fake<ISeasonFactory>();
            var fakeSeasonLookup = A.Fake<ISeasonLookup>();
            var fakeUrlHelper = A.Fake<IUrlHelper>();

            A.CallTo(() => fakeUrlHelper.SeasonUrl).Returns(@"http://localhost:8082/anime/season/{0}/{1}");

            var instance = new SeasonRetriever(fakeFactory, fakeSeasonLookup, fakeUrlHelper);
            var result = instance.GetSeasonData(year, season).Result;

            Assert.AreEqual(result.Count, 78);
            var first = result.FirstOrDefault();
            Assert.IsNotNull(first);
            Assert.AreEqual(first.Id, 31737);
            Assert.AreEqual(first.Title, "Gakusen Toshi Asterisk 2nd Season");
        }

        [Test]
        public void TestCurrentSeason()
        {
            const int year = 2017;
            var seasons = new[] {"Winter", "Spring", "Summer"};

            var fakeFactory = A.Fake<ISeasonFactory>();
            var fakeSeasonLookup = A.Fake<ISeasonLookup>();
            var fakeUrlHelper = A.Fake<IUrlHelper>();

            var httpMock = HttpMockRepository.At("http://localhost:8082");
            var path = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var season in seasons)
            {
                var file = Path.Combine(path, "AnimeExamples", $"{2016}{season}.html");
                var content = File.ReadAllText(file);
                httpMock.Stub(x => x.Get($"/anime/season/{year}/{season.ToLower(CultureInfo.InvariantCulture)}"))
                    .Return(content)
                    .OK();
            }


            A.CallTo(() => fakeUrlHelper.SeasonUrl).Returns(@"http://localhost:8082/anime/season/{0}/{1}");

            A.CallTo(() => fakeSeasonLookup.CalculateCurrentSeason(A<DateTime>._)).Returns("Winter");
            A.CallTo(() => fakeSeasonLookup.GetNextSeason("Winter")).Returns("Spring");
            A.CallTo(() => fakeSeasonLookup.GetNextSeason("Spring")).Returns("Summer");
            A.CallTo(() => fakeSeasonLookup.NextSeasonYear("Winter", year)).Returns(year);
            A.CallTo(() => fakeSeasonLookup.NextSeasonYear("Spring", year)).Returns(year);

            var instance = new SeasonRetriever(fakeFactory, fakeSeasonLookup, fakeUrlHelper);
            var result = instance.RetrieveCurrentSeason().Result;

            Assert.AreEqual(result.Count, 299);


        }

    }
}
