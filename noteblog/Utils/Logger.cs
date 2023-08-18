using System;
using log4net;

namespace noteblog.Utils
{
    public class Logger
    {
        private ILog log;

        public Logger(string className)
        {
            // log4net init
            log4net.Config.XmlConfigurator.Configure();

            log = LogManager.GetLogger(className);
        }

        public void Debug(string message)
        {
            log.Debug(message);
        }

        public void Info(string message)
        {
            log.Info(message);
        }

        public void Warn(string message)
        {
            log.Warn(message);
        }

        public void Error(string message, Exception ex)
        {
            log.Error(message, ex);
        }

        public void Fatal(string message)
        {
            log.Fatal(message);
        }

        public void Shutdown()
        {
            log.Logger.Repository.Shutdown();
        }

    }
}