namespace MAL.NetSelfHosted.Enumerations
{
    public static class ConfigurationKeys
    {
        /// <summary>
        /// Indicate if logging should be done to the console
        /// </summary>
        public const string LogToConsole = "LogToConsole";

        /// <summary>
        /// Indicate if logging should be done to a file
        /// </summary>
        public const string LogToFile = "LogToFile";

        /// <summary>
        /// Path where log file should be written to
        /// </summary>
        public const string LogPath = "LogPath";

        /// <summary>
        /// Indicate if logging should be done to Splunk
        /// </summary>
        public const string LogToSplunk = "LogToSplunk";

        /// <summary>
        /// Url of the Splunk Event Collector
        /// </summary>
        public const string SplunkUrl = "SplunkUrl";

        /// <summary>
        /// Event Collector token for Splunk
        /// </summary>
        public const string SplunkToken = "SplunkToken";
    }
}