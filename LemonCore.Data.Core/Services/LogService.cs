using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LemonCore.Core.Services
{
    public class LogService
    {
        private ILog _logger;
        public static LogService Default;
        public static ILoggerRepository Repository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        static LogService()
        {
            LogService.Default = new LogService();
            LogService.SetupLog4Net();
        }

        private LogService()
        {
            this._logger = GetLog("lemon.log");
        }

        public ILog GetLog(string name)
        {
            return GetLog(Repository.Name, name);
        }
        public ILog GetLog(string repository, string name)
        {
            return LogManager.GetLogger(repository, name);
        }


        protected static void SetupLog4Net()
        {
            Hierarchy hierarchy = (Hierarchy)Repository;
            PatternLayout patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };

            patternLayout.ActivateOptions();

            RollingFileAppender rollingFileAppender = new RollingFileAppender
            {
                AppendToFile = true,
                File = @"logs\",
                Layout = patternLayout,
                MaxSizeRollBackups = 5,
                DatePattern = "yyyyMMddHHmmss'.log'",
                MaximumFileSize = "1GB",
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                StaticLogFileName = false
            };

            rollingFileAppender.ActivateOptions();

            hierarchy.Root.AddAppender(rollingFileAppender);
            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
        }

        public void Info(string message)
        {
            this._logger.Info(message);
        }

        public void Error(string message, Exception ex = null)
        {
            this._logger.Error(message, ex);
        }

        public void Error(string message)
        {
            this._logger.Error(message);
        }
    }
}
