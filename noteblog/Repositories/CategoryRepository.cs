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

    public bool insert(string name, string description = null)
    {
        return _dbConnection.Execute("INSERT INTO categories (name, description) VALUES (@name, @description)", new { name, description }) == 1;
    }

    public bool update(int id, string name = "", string description = null)
    {
        return _dbConnection.Execute("UPDATE categories SET name = @name, description = @description WHERE id = @id", new { name, description, id }) == 1;
    }

    public bool delete(int id)
    {
        return _dbConnection.Execute("DELETE FROM categories WHERE id = @id", new { id }) >= 0;
    }
}
