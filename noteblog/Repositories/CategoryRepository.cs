using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using noteblog.Models;
using System.Linq;

public class CategoryRepository
{
    private readonly IDbConnection _dbConnection;

    public CategoryRepository()
    {
        _dbConnection = DatabaseHelper.GetConnection();
    }

    public List<Category> getAll()
    {
        var categories = _dbConnection.Query<Category>("SELECT * FROM categories").ToList();
        return categories;
    }

    public int getId(string name)
    {
        return _dbConnection.QuerySingle<int>("SELECT id FROM categories WHERE name = @name", new { name });
    }
}
