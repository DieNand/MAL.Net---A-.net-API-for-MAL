using MAL.NetSelfHosted.Classes;
using NUnit.Framework;

namespace MAL.NetTests.SelfhostedTests
{
    public class ConfigurationHelperTest
    {
        [Test]
        public void TestConfigLoad()
        {
            const string keyToRetrieve = "Port";
            var typedKey = keyToRetrieve.GetConfigurationValue<int>();

            Assert.That(typedKey.GetType(), Is.EqualTo(typeof(int)));
        }
    }
}