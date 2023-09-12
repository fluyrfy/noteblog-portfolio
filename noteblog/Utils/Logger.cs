using System;
using System.Data;
using Dapper;
using log4net;

namespace noteblog.Utils
{
    public class Logger
    {
        private readonly IDbConnection _dbConnection;
        private ILog log;
        private LogRepository LR;

        public Logger(string className)
        {
            // log4net init
            log4net.Config.XmlConfigurator.Configure();

            log = LogManager.GetLogger(className);
            _dbConnection = DatabaseHelper.GetConnection();
            LR = new LogRepository();
        }

        public void Debug(string message)
        {
            LR.insert("debug", message);
        }

        public void Info(string message)
        {
            LR.insert("info", message);
        }

        public void Warn(string message)
        {
            LR.insert("warn", message);
        }

        public void Error(string message, Exception ex)
        {
            LR.insert("error", message, ex.ToString());
        }

        public void Fatal(string message)
        {
            LR.insert("fatal", message);
        }

        public void Shutdown()
        {
            log.Logger.Repository.Shutdown();
        }

    }
}