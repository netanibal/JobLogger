using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Job
{
    [TestClass]
    public class LoggerTest
    {
        /// <summary>
        /// Incorrectly configured constructor
        /// </summary>
        [TestMethod]
        public void Constructor_ConfigurationFail()
        {
            var component = new Logger(logToConsole: false, logToDatabase: false, logToFile: false);

            Assert.ThrowsException<ArgumentException>(() => Logger.LogMessage("test", Logger.LogLevel.Error));
        }

        /// <summary>
        /// Incorrectly configured constructor
        /// </summary>
        [TestMethod]
        public void Constructor_Fail()
        {
            var component = new Logger(logLevels: Logger.LogLevel.None);

            Assert.ThrowsException<ArgumentNullException>(() => Logger.LogMessage("test", Logger.LogLevel.Error));
        }

        /// <summary>
        /// Test LogMessage wrong parameter
        /// </summary>
        [TestMethod]
        public void LogMessage_CheckParametersFail()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Logger.LogMessage("test", Logger.LogLevel.None));
        }

        /// <summary>
        /// String empty without error
        /// </summary>
        [TestMethod]
        public void LogMessage_Empty()
        {
            Logger.LogMessage(string.Empty, Logger.LogLevel.Error);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// String with spaces without error
        /// </summary>
        [TestMethod]
        public void LogMessage_Spaces()
        {
            Logger.LogMessage("   ", Logger.LogLevel.Error);
            Assert.IsTrue(true);
        }

    }
}
