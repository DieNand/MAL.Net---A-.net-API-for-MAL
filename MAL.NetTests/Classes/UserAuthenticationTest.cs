using System;
using System.IO;
using System.Net;
using System.Reflection;
using FakeItEasy;
using MAL.NetLogic.Classes;
using MAL.NetLogic.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MAL.NetTests.Classes
{
    [TestClass]
    public class UserAuthenticationTest
    {
        [TestMethod]
        public void TestAuthentication()
        {
            var csrfToken = "f301b7983b15c0a3340e2e6f17731c8e4f18b872";

            var fakeFactory = A.Fake<IAuthFactory>();
            var fakeLoginData = A.Fake<ILoginData>();
            var fakeWebFactory = A.Fake<IWebHttpWebRequestFactory>();
            var fakeWebRequest = A.Fake<IWebHttpWebRequest>();
            var loginFakeRequest = A.Fake<IWebHttpWebRequest>();
            var fakeConsole = A.Fake<IConsoleWriter>();

            A.CallTo(() => fakeFactory.CreateLingData()).Returns(fakeLoginData);
            A.CallTo(() => fakeWebFactory.Create()).ReturnsNextFromSequence(fakeWebRequest, loginFakeRequest);

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine("AnimeExamples", $"login.html");
            Assert.IsNotNull(path);
            var fullPath = Path.Combine(path, file);

            var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read);
            A.CallTo(() => loginFakeRequest.GetResponseStream()).Returns(stream);

            var wStream = new MemoryStream();
            A.CallTo(() => fakeWebRequest.GetRequestStream()).Returns(wStream);

            A.CallTo(() => fakeWebRequest.StatusCode).Returns(HttpStatusCode.OK);

            var rFile = Path.Combine("AnimeExamples", "28999.html");
            var rFullPath = Path.Combine(path, rFile);

            var rStream = File.Open(rFullPath, FileMode.Open, FileAccess.Read);
            A.CallTo(() => fakeWebRequest.GetResponseStream()).Returns(rStream);

            var instance = new UserAuthentication(fakeFactory, fakeWebFactory, fakeConsole);
            var result = instance.Login("testUsr", "testPass");
            
            Assert.IsNotNull(fakeWebRequest.CookieContainer);
            Assert.AreEqual("yourMalUserAgentStringHere", fakeWebRequest.UserAgent);
            Assert.AreEqual(WebRequestMethods.Http.Post, fakeWebRequest.Method);
            Assert.AreEqual("application/x-www-form-urlencoded", fakeWebRequest.ContentType);

            A.CallTo(() => fakeWebRequest.CreateRequest(@"http://myanimelist.net/login.php")).MustHaveHappened(Repeated.Exactly.Once);

            Assert.IsTrue(result.LoginValid);
            Assert.IsNotNull(result.Cookies);
        }
    }
}
