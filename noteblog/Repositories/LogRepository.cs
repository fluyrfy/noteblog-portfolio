using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.FluentMap;
using noteblog.Models;
using noteblog.Models.Mappings;

public class LogRepository
{
    private readonly IDbConnection _dbConnection;

    public LogRepository()
    {
        _dbConnection = DatabaseHelper.GetConnection();
        FluentMapper.EntityMaps.Clear();
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new UserMap());
        });
    }


    public List<User> getAll()
    {
        string query = "SELECT * FROM users ORDER BY updated_at DESC";
        return _dbConnection.Query<User>(query).ToList();
    }

    public int getTotalCount(int categoryId)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SELECT COUNT(*) FROM notes");
        if (categoryId > 0)
        {
            sb.AppendLine("WHERE category_id = @categoryId");
        }
        string query = sb.ToString();
        var parameters = new { categoryId = categoryId > 0 ? categoryId : (int?)null };
        int result = _dbConnection.QuerySingleOrDefault<int>(query, parameters);
        if (result == null)
        {
            result = 0;
        }
        return result;
    }

    public bool insert(string level, string message, string exception = "")
    {
        using(_dbConnection)
        {
            string query = "INSERT INTO logs (level, message, exception) VALUES (@level, @message, @exception)";
            return _dbConnection.Execute(query, new { level, message, exception }) == 1;
        }
    }

    public bool delete(DateTime expiredTime)
    {
        using (_dbConnection)
        {
            string query = "DELETE FROM logs WHERE created_at < @expiredTime";
            return _dbConnection.Execute(query, new { expiredTime }) >= 0;
        }
    }
}
