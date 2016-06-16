using System;
using System.Configuration;

namespace MAL.NetSelfHosted.Classes
{
    public static class ConfigurationHelper
    {
        public static T GetConfigurationValue<T>(this string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            var typedValue = (T) Convert.ChangeType(value, typeof(T));
            return typedValue;
        }
    }
}