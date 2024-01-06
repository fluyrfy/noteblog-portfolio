using System;
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

  public List<User> getAll(out int totalRecords, out int nowSetRecords, int nowPage = 0, int limit = 5)
  {
    using (_dbConnection)
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("SELECT * FROM users ORDER BY updated_at DESC");
      totalRecords = _dbConnection.ExecuteScalar<int>(DatabaseHelper.GetTotalRecords(sb.ToString()));
      if (nowPage != 0)
      {
        sb.AppendLine("LIMIT @limit OFFSET @offset");
      }
      int offset = (nowPage - 1) * limit;
      nowSetRecords = _dbConnection.ExecuteScalar<int>(DatabaseHelper.GetTotalRecords(sb.ToString()), new { limit, offset });
      return _dbConnection.Query<User>(sb.ToString(), new { limit, offset }).ToList();
    }
  }
  public List<string> getAllEmail()
  {
    using (_dbConnection)
    {
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("SELECT email FROM users ORDER BY updated_at DESC");
      return _dbConnection.Query<string>(sb.ToString()).ToList();
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
  public List<UserSkill> getSkills(string userId)
  {
    string sql = "SELECT * FROM user_skills WHERE user_id = @userId";
    return _dbConnection.Query<UserSkill>(sql, new { userId }).ToList();
  }
  public User get(string id = null, string role = "root")
  {
    if (string.IsNullOrEmpty(id))
    {
      return _dbConnection.QueryFirstOrDefault<User>("SELECT * FROM users WHERE role = @role", new { role });
    }
    return _dbConnection.QueryFirstOrDefault<User>("SELECT * FROM users WHERE id = @id", new { id });
  }

  public List<User> getByKeyword(string text)
  {
    return _dbConnection.Query<User>("SELECT * FROM users WHERE name LIKE CONCAT('%', @text, '%') OR email LIKE CONCAT('%', @text, '%')", new { text }).ToList();
  }


  public bool insert(Draft draft)
  {
    string query = "INSERT INTO drafts (title, content, keyword, development, pic, user_id, note_id) VALUES (@title, @content, @keyword, @development, @pic, @userId, @noteId)";
    int rowsAffected = _dbConnection.Execute(query, draft);
    return rowsAffected == 1;
  }

  public void update(string name, byte[] avatar, byte[] resume, string id, string phone = "", string region = "", string regionLink = "", string githubLink = "", string jobTitle = "", string about = "")
  {
    string query;
    var parameters = new { name, phone, region, regionLink, githubLink, jobTitle, about, id };
    query = "UPDATE users SET name = @name, phone = @phone, region = @region, region_link = @regionLink, github_link = @githubLink, job_title = @jobTitle, biography = @about WHERE id = @id";
    _dbConnection.Execute(query, parameters);
    if (avatar != null && avatar.Length > 0)
    {
      query = "UPDATE users SET avatar = @avatar WHERE id = @id";
      var paramAvatar = new { avatar, id };
      _dbConnection.Execute(query, paramAvatar);
    }
    if (resume != null && resume.Length > 0)
    {
      query = "UPDATE users SET resume = @resume WHERE id = @id";
      var paramResume = new { resume, id };
      _dbConnection.Execute(query, paramResume);
    }
  }

  public void updateSkill(string id, string skillOneName = "", string skillOnePercent = "", string skillTwoName = "", string skillTwoPercent = "", string skillThreeName = "", string skillThreePercent = "")
  {
    string sql = "INSERT INTO user_skills (user_id, skill_id, name, percent) VALUES (@id, @skillId, @name, @percent) ON DUPLICATE KEY UPDATE name=@name, percent=@percent";
    var skills = new[] {
          new { id= 1, name = skillOneName, percent = skillOnePercent },
          new { id = 2, name = skillTwoName, percent = skillTwoPercent },
          new { id = 3, name = skillThreeName, percent = skillThreePercent }
        };
    foreach (var skill in skills)
    {
      _dbConnection.Execute(sql, new { id, skillId = skill.id, name = skill.name, percent = skill.percent });
    }
  }

  public void updateRole(string role, string id)
  {
    string query = "UPDATE users SET role = @role WHERE id = @id";
    _dbConnection.Execute(query, new { role, id });
  }

  public bool delete(string id)
  {
    string query = "DELETE FROM users WHERE id = @id";
    return _dbConnection.Execute(query, new { id }) >= 0;
  }
}
