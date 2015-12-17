using System;
using System.Configuration;
using System.IO;

namespace MAL.NetLogic.Interfaces
{
    public class LogWriter : ILogWriter
    {
        #region Variables

        private string _logPath;

        #endregion

        #region Constructor

        public LogWriter()
        {
            loadLogPath();
        }

        #endregion

        #region Public Methods

        public void WriteLogData(string message)
        {
            if (File.Exists(_logPath))
            {
                using (var file = File.AppendText(_logPath))
                {
                    file.WriteLine($"[{DateTime.Now}] - {message}");
                }
            }
            else
            {
                loadLogPath();
                WriteLogData(message);
            }
        }

        #endregion

        #region Private Methods

        private void loadLogPath()
        {
            _logPath = ConfigurationManager.AppSettings["LogPath"];
            if (File.Exists(_logPath)) return;
            using (var file = File.CreateText(_logPath))
            {
                file.WriteLine($"[{DateTime.Now}] - Log created");
            }
        }

        #endregion
    }
}