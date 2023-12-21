using System.Collections.Generic;
using System.Data;
using System.Linq;
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

  public List<User> getCoAuthor(int noteId)
  {
    var coAuthors = _dbConnection.Query<User>(
      @"SELECT u.* FROM `note_co-authors` nca
        JOIN users u ON nca.user_id = u.id
        WHERE nca.note_id = @noteId",
      new { noteId }
    ).ToList();
    return coAuthors;
  }
  public void insertCoAuthor(int noteId, int userId)
  {
    string query = "INSERT INTO `note_co-authors` (note_id, user_id) VALUES (@noteId, @userId)";
    _dbConnection.Execute(query, new { noteId, userId });
  }

  public void deleteCoAuthor(int noteId)
  {
    string query = "DELETE FROM `note_co-authors` WHERE note_id = @noteId";
    _dbConnection.Execute(query, new { noteId });
  }
}
