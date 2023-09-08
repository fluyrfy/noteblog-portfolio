using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.FluentMap;
using noteblog.Models;
using noteblog.Models.Mappings;

public class UserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository()
    {
        _dbConnection = DatabaseHelper.GetConnection();
        FluentMapper.EntityMaps.Clear();
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new UserMap());
        });
    }

    //public bool isUserExist(int userId, int noteId)
    //{
    //    string query = "SELECT COUNT(*) FROM notes WHERE user_id = @userId AND note_id = @noteId";
    //    int count = _dbConnection.ExecuteScalar<int>(query, new { userId, noteId });
    //    return count > 0;
    //}

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

    public User get(int id)
    {
        return _dbConnection.QueryFirst<User>("SELECT * FROM users WHERE id = @id", new { id });
    }


    public bool insert(Draft draft)
    {
        string query = "INSERT INTO drafts (title, content, keyword, development, pic, user_id, note_id) VALUES (@title, @content, @keyword, @development, @pic, @userId, @noteId)";
        int rowsAffected = _dbConnection.Execute(query, draft);
        return rowsAffected == 1;
    }

    public bool update(string name, byte[] avatar, string role, int id)
    {
        string query;
        if (avatar != null && avatar.Length > 0)
        {
            var parameters = new { name, avatar, role, id };
            query = "UPDATE users SET name = @name, avatar = @avatar, role = @role WHERE id = @id";
            return _dbConnection.Execute(query, parameters) == 1;
        }
        else
        {
            var parameters = new { name, role, id };
            query = "UPDATE users SET name = @name, role = @role WHERE id = @id";
            return _dbConnection.Execute(query, parameters) == 1;
        }
    }

    public bool delete(int id)
    {
        string query = "DELETE FROM users WHERE id = @id";
        return _dbConnection.Execute(query, new { id }) >= 0;
    }
}
