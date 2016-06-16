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
    }
}