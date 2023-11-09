using System;
using System.Data;
using System.Linq;
using Dapper;
using Dapper.FluentMap;
using noteblog.Models;
using noteblog.Models.Mappings;

public class DraftRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly int _userId;


    public DraftRepository(int userId)
    {
        _dbConnection = DatabaseHelper.GetConnection();
        _userId = userId;
        FluentMapper.EntityMaps.Clear();
        FluentMapper.Initialize(config =>
        {
            config.AddMap(new DraftMap());
        });

    }

    public bool isDraftExist(int userId, int noteId)
    {
        string query = "SELECT COUNT(*) FROM drafts WHERE user_id = @userId AND note_id = @noteId";
        int count = _dbConnection.ExecuteScalar<int>(query, new { userId, noteId });
        return count > 0;
    }

    public Draft get(int userId, int noteId)
    {
        string query = "SELECT * FROM drafts WHERE user_id = @userId AND note_id = @noteId";
        var result = _dbConnection.Query<Draft>(query, new { userId, noteId }).FirstOrDefault();

        return _dbConnection.QueryFirstOrDefault<Draft>(query, new { userId, noteId });
    }

    public bool insert(Draft draft)
    {
        string query = "INSERT INTO drafts (category_id, title, content, keyword, pic, user_id, note_id) VALUES (@categoryId, @title, @content, @keyword, @pic, @userId, @noteId)";
        int rowsAffected = _dbConnection.Execute(query, draft);
        return rowsAffected == 1;
    }

    public bool update(Draft draft)
    {
        string query = "UPDATE drafts SET category_id = @categoryId, title = @title, content = @content, keyword = @keyword, pic = @pic WHERE user_id = @userId AND note_id = @noteId";
        return _dbConnection.Execute(query, draft) == 1;
    }

    public bool delete(int userId, int noteId)
    {
        try
        {
            string query = "DELETE FROM drafts WHERE user_id = @userId AND note_id = @noteId";
            int rowsAffected = _dbConnection.Execute(query, new { userId, noteId });
            return rowsAffected >= 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
