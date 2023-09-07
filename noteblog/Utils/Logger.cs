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

        public Logger(string className)
        {
            // log4net init
            log4net.Config.XmlConfigurator.Configure();

            log = LogManager.GetLogger(className);
            _dbConnection = DatabaseHelper.GetConnection();
        }

        public void Debug(string message)
        {
            //log.Debug(message);
            string query = "INSERT INTO logs (level, message) VALUES ('debug', @message)";
            _dbConnection.Execute(query, new { message });
        }

        public void Info(string message)
        {
            //log.Info(message);
            string query = "INSERT INTO logs (level, message) VALUES ('info', @message)";
            _dbConnection.Execute(query, new { message });
        }

        public void Warn(string message)
        {
            //log.Warn(message);
            string query = "INSERT INTO logs (level, message) VALUES ('warn', @message)";
            _dbConnection.Execute(query, new { message });
        }

        public void Error(string message, Exception ex)
        {
            //log.Error(message, ex);
            string query = "INSERT INTO logs (level, message, exception) VALUES ('error', @message, @ex)";
            _dbConnection.Execute(query, new { message, ex = ex.ToString() });
        }

        public void Fatal(string message)
        {
            //log.Fatal(message);
            string query = "INSERT INTO logs (level, message) VALUES ('fatal', @message)";
            _dbConnection.Execute(query, new { message });
        }

        public void Shutdown()
        {
            log.Logger.Repository.Shutdown();
        }

    }
}