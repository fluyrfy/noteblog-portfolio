using System;
using System.Data;
using System.Linq;
using Dapper;
using Dapper.FluentMap;
using noteblog.Models;
using noteblog.Models.Mappings;
using noteblog.Utils;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

public class DraftRepository
{
  private readonly IDbConnection _dbConnection;
  private readonly string _userId;
  private Logger _log;


  public DraftRepository(string userId)
  {
    _dbConnection = DatabaseHelper.GetConnection();
    _userId = userId;
    _log = new Logger(typeof(DraftRepository).Name);
    FluentMapper.EntityMaps.Clear();
    FluentMapper.Initialize(config =>
    {
      config.AddMap(new DraftMap());
    });

  }

  public bool isDraftExist(string userId, int noteId)
  {
    string query = "SELECT COUNT(*) FROM drafts WHERE user_id = @userId AND note_id = @noteId";
    int count = _dbConnection.ExecuteScalar<int>(query, new { userId, noteId });
    return count > 0;
  }

  public Draft get(string userId, int noteId)
  {
    string query = "SELECT * FROM drafts WHERE user_id = @userId AND note_id = @noteId";
    var result = _dbConnection.Query<Draft>(query, new { userId, noteId }).FirstOrDefault();

    return _dbConnection.QueryFirstOrDefault<Draft>(query, new { userId, noteId });
  }

  public bool insert(Draft draft)
  {
    try
    {
      string query = "INSERT INTO drafts (category_id, title, content, keyword, pic, `co-author`,  user_id, note_id) VALUES (@categoryId, @title, @content, @keyword, @pic, @coAuthor, @userId, @noteId)";
      _log.Debug($"Insert draft info - user_id: {draft.userId}, note_id: {draft.noteId}, title: {draft.title}");
      int rowsAffected = _dbConnection.Execute(query, draft);
      return rowsAffected == 1;
    }
    catch (Exception ex)
    {
      _log.Error("Failed to insert draft", ex);
      throw;
    }
  }

  public bool update(Draft draft)
  {
    try
    {
      string query = "UPDATE drafts SET category_id = @categoryId, title = @title, content = @content, keyword = @keyword, pic = @pic, `co-author` = @coAuthor WHERE user_id = @userId AND note_id = @noteId";
      _log.Debug($"Update draft info - user_id: {draft.userId}, note_id: {draft.noteId}, title: {draft.title}");
      return _dbConnection.Execute(query, draft) == 1;
    }
    catch (System.Exception ex)
    {
      _log.Error("Failed to update draft", ex);
      throw;
    }
  }

  public bool delete(string userId, int noteId)
  {
    try
    {
      string query = "DELETE FROM drafts WHERE user_id = @userId AND note_id = @noteId";
      int rowsAffected = _dbConnection.Execute(query, new { userId, noteId });
      _log.Debug($"Delete draft info - user_id: {userId}, note_id: {noteId}");
      return rowsAffected >= 0;
    }
    catch (Exception ex)
    {
      _log.Error("Failed to delete draft", ex);
      return false;
    }
  }
}
