﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using noteblog.Models;
using noteblog.Utils;

namespace noteblog
{
  public partial class Take : Page
  {
    private Logger log;
    protected void Page_Load(object sender, EventArgs e)
    {
      Response.Cache.SetCacheability(HttpCacheability.NoCache); //Cache-Control : no-cache, Pragma : no-cache
      Response.Cache.SetExpires(DateTime.Now.AddDays(-1)); //Expires : date time
      Response.Cache.SetNoStore(); //Cache-Control :  no-store
      Response.Cache.SetProxyMaxAge(new TimeSpan(0, 0, 0)); //Cache-Control: s-maxage=0
      Response.Cache.SetValidUntilExpires(false);
      Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);//Cache-Control:  must-revalidate
      log = new Logger(typeof(Take).Name);
      if (!IsPostBack)
      {
        CategoryRepository categoryRepository = new CategoryRepository();
        List<Category> categories = categoryRepository.getAll(out int tr, out int nsr, 1, 0);

        foreach (Category category in categories)
        {
          ListItem item = new ListItem(category.name, category.id.ToString());
          rdlCategory.Items.Add(item);
        }
      }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
      if (Page.IsValid && AuthenticationHelper.IsUserAuthenticatedAndTicketValid())
      {
        var userId = AuthenticationHelper.GetUserId();
        int maxFileSizeInBytes = 1024 * 1024;
        if (fuCoverPhoto.HasFile)
        {
          if (fuCoverPhoto.PostedFile.ContentLength > maxFileSizeInBytes)
          {
            lblPhotoMsg.Text = "Image size exceeds limit（1MB）";
            lblPhotoMsg.ForeColor = System.Drawing.Color.Red;
            return;
          }
        }
        using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
        {
          var newTitle = "";
          var newCategory = "";
          var newId = "";
          try
          {
            log.Info("Starting to create new note");
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO notes(user_id, category_id, title, content, content_text, keyword, published_at, pic) VALUES (@userId, @categoryId, @title, @content, @contentText, @keyword, @publishedAt, @pic)";
            var title = HttpUtility.HtmlEncode(txtTitle.Text);
            newTitle = title;
            var content = HttpUtility.HtmlEncode(hdnContent.Value);
            var keyword = HttpUtility.HtmlEncode(txtKeyword.Text);
            cmd.Parameters.AddWithValue("@userId", userId);
            cmd.Parameters.AddWithValue("@categoryId", rdlCategory.SelectedValue);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@content", content);
            cmd.Parameters.AddWithValue("@contentText", ConverterHelper.ExtractTextFromHtml(txtContent.Text));
            cmd.Parameters.AddWithValue("@keyword", keyword);
            cmd.Parameters.AddWithValue("@publishedAt", DateTime.UtcNow);
            byte[] imgData = new byte[0];
            imgData = !string.IsNullOrEmpty(hdnImgData.Value) ? Convert.FromBase64String(hdnImgData.Value) : null;

            cmd.Parameters.AddWithValue("@pic", imgData);
            log.Debug($"New note info - title: {title}");
            con.Open();
            cmd.ExecuteNonQuery();
            MySqlCommand cmd2 = new MySqlCommand();
            cmd2.Connection = con;
            cmd2.CommandText = "SELECT LAST_INSERT_ID()";
            newId = cmd2.ExecuteScalar().ToString();
            string[] hdnArray = hdnSelectedCoAuthorUserIds.Value.Split(',');
            new NoteRepository().deleteCoAuthor(Convert.ToInt32(newId));
            foreach (string coAuthorId in hdnArray)
            {
              new NoteRepository().insertCoAuthor(Convert.ToInt32(newId), coAuthorId);
            }
            newCategory = new CategoryRepository().get(Convert.ToInt32(rdlCategory.SelectedValue)).name;
            log.Info("Note created successfully, note ID: " + newId);
            DraftRepository draftRepository = new DraftRepository(userId);
            draftRepository.delete(userId, 0);
            CacheHelper.ClearAllCache();
          }
          catch (Exception ex)
          {
            log.Error("Failed to create new note", ex);
            throw;
          }
          log.Info("End of note creation method");
          // List<string> emailList = new UserRepository().getAllEmail();
          // foreach (string email in emailList)
          // {
          //   EmailHelper.SendEmail($"{email}", "New note on F.L. - check it out!", "New Note Alert", $"Don’t miss the new note on F.L.: {newTitle}. It’s about {newCategory}. Enjoy it.", "read it", $"Note?id={newId}");
          // }
          Response.Redirect("Dashboard.aspx");

        }
      }
    }
  }
}