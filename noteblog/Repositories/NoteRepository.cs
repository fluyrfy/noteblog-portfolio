using System.Data;
using System.Text;
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

    public Note getLatestNote()
    {
        using (_dbConnection)
        {
            string query = "SELECT * FROM notes ORDER BY updated_at DESC;";
            return _dbConnection.QueryFirstOrDefault<Note>(query);
        }
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


}
