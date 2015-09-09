using System;
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
            var fakeFactory = A.Fake<IAuthFactory>();
            var fakeLoginData = A.Fake<ILoginData>();

            A.CallTo(() => fakeFactory.CreateLingData()).Returns(fakeLoginData);

            var instance = new UserAuthentication(fakeFactory);
            var result = instance.Login("testUsr", "testPass");
            result.RunSynchronously();

        }
    }
}
