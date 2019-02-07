using System;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace Job
{
    public class Logger
    {
        /// <summary>
        /// Enumeration to define the log level
        /// </summary>
        public enum LogLevel
        {
            None = 0,
            Message = 1,
            WarningError = 2,
            Error = 3,
        }

        private static bool _logToFile;
        private static bool _logToConsole;
        private static bool _logToDatabase;
        private static LogLevel _logLevel;

        /// <summary>
        /// Set up the logger.
        /// </summary>
        /// <param name="logToFile">Log in file.</param>
        /// <param name="logToConsole">Log in console.</param>
        /// <param name="logToDatabase">Log in database.</param>
        /// <param name="logLevels">Message log level.</param>
        public Logger(bool logToFile = false, bool logToConsole = true, bool logToDatabase = false, LogLevel logLevels = LogLevel.Error)
        {
            _logToDatabase = logToDatabase;
            _logToFile = logToFile;
            _logToConsole = logToConsole;
            _logLevel = logLevels;
        }
        /// <summary>
        /// Write the message to the configured destination.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel">Message log level.</param>
        public static void LogMessage(string message, LogLevel logLevel)
        {
            if (!Validate(message, logLevel))
            {
                return;
            }

            if (LogOn(logLevel))
            {
                if (_logToDatabase)
                {
                    LogDatabase(message, logLevel);
                }

                if (_logToFile)
                {
                    LogArchive(message, logLevel);
                }

                if (_logToConsole)
                {
                    LogConsole(message, logLevel);
                }
            }
        }
        /// <summary>
        /// Validates that the configured values and parameters of the main method are correct.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel">Message Log level.</param>
        /// <returns></returns>
        private static bool Validate(string message, LogLevel logLevel)
        {
            //Validate message
            message.Trim();
            if (message == null || message.Length == 0)
            {
                return false;
            }

            //Validate destination
            if (!_logToConsole && !_logToFile && !_logToDatabase)
            {
                throw new ArgumentException("Invalid configuration");
            }

            //Validate log level
            if (logLevel == LogLevel.None || _logLevel == LogLevel.None)
            {
                throw new ArgumentNullException("Error or Warning or Message must be specified");
            }
            return true;
        }
        /// <summary>
        /// Return logged.
        /// </summary>
        /// <param name="logLevel">Message Log level.</param>
        /// <returns></returns>
        private static bool LogOn(LogLevel logLevel)
        {
            bool logOn = false;

            switch (_logLevel)
            {
                case LogLevel.Message:
                    logOn = logLevel == LogLevel.Message;
                    break;
                case LogLevel.WarningError:
                    logOn = logLevel >= LogLevel.Message;
                    break;
                case LogLevel.Error:
                    logOn = logLevel == LogLevel.Error;
                    break;
            }

            return logOn;
        }
        /// <summary>
        /// Log in database.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel">Message Log level.</param>
        private static void LogDatabase(string message, LogLevel logLevel)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandText = "Insert into Log Values (@MENSSAGE, @LEVEL)";
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("@MENSSAGE", message);
                    command.Parameters.AddWithValue("@LEVEL", logLevel.ToString());
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        /// <summary>
        /// Log in Archive
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel">Message Log level.</param>
        private static void LogArchive(string message, LogLevel logLevel)
        {
            System.Text.StringBuilder logText = new System.Text.StringBuilder();
            string file = ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt";

            if (!File.Exists(file))
            {
                logText.AppendLine(File.ReadAllText(file));
            }

            logText.AppendLine(string.Format("{0} {1} {2}", DateTime.Now.ToShortDateString(), logLevel.ToString(), message));

            File.WriteAllText(file, logText.ToString());
        }
        /// <summary>
        /// Log in Console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel">Message Log level.</param>
        private static void LogConsole(string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Message:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.WarningError:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    break;
            }
            Console.WriteLine(DateTime.Now.ToShortDateString() + message);
        }
    }
}
