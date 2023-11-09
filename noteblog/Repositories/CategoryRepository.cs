using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using noteblog.Models;

public class CategoryRepository
{
    private readonly IDbConnection _dbConnection;

    public CategoryRepository()
    {
        _dbConnection = DatabaseHelper.GetConnection();
    }

    public List<Category> getAll(out int totalRecords, out int nowSetRecords, int nowPage = 1, int limit = 5)
    {
        using (_dbConnection)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM categories");
            totalRecords = _dbConnection.ExecuteScalar<int>(DatabaseHelper.GetTotalRecords(sb.ToString()));
            sb.AppendLine("LIMIT @limit OFFSET @offset");
            int offset = (nowPage - 1) * limit;
            var parameters = new { limit, offset };
            nowSetRecords = _dbConnection.ExecuteScalar<int>(DatabaseHelper.GetTotalRecords(sb.ToString()), parameters);
            var categories = _dbConnection.Query<Category>(sb.ToString(), parameters).ToList();
            return categories;
        }
    }

    public int getId(string name)
    {
        return _dbConnection.QuerySingle<int>("SELECT id FROM categories WHERE name = @name", new { name });
    }

    public Category get(int id)
    {
        return _dbConnection.QuerySingle<Category>("SELECT * FROM categories WHERE id = @id", new { id });
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
