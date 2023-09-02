using System.Data;
using Dapper;
using noteblog.Models;

public class NoteRepository
{
    private readonly IDbConnection _dbConnection;

    public NoteRepository()
    {
        _dbConnection = DatabaseHelper.GetConnection();
    }

    public bool isNoteExist(int userId, int noteId)
    {
        string query = "SELECT COUNT(*) FROM notes WHERE user_id = @userId AND note_id = @noteId";
        int count = _dbConnection.ExecuteScalar<int>(query, new { userId, noteId });
        return count > 0;
    }

    public Draft get(int userId, int noteId)
    {
        string query = "SELECT * FROM drafts WHERE user_id = @userId AND note_id = @noteId";
        return _dbConnection.QueryFirstOrDefault<Draft>(query, new { userId, noteId });
    }

    public bool insert(Draft draft)
    {
        string query = "INSERT INTO drafts (title, content, keyword, development, pic, user_id, note_id) VALUES (@title, @content, @keyword, @development, @pic, @userId, @noteId)";
        int rowsAffected = _dbConnection.Execute(query, draft);
        return rowsAffected == 1;
    }

    public bool update(Draft draft)
    {
        string query = "UPDATE drafts SET title = @title, content = @content, keyword = @keyword, pic = @pic, development = @development WHERE id = @id";
        return _dbConnection.Execute(query, draft) == 1;
    }

    public int delete(int id)
    {
        string query = "DELETE FROM drafts WHERE id = @id";
        return _dbConnection.Execute(query, new { id = id });
    }
}
