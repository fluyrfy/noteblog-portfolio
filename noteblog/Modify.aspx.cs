﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using noteblog.Utils;

namespace noteblog
{
    public partial class Modify : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string noteIdString = Request.QueryString["id"];
                if (string.IsNullOrEmpty(noteIdString))
                {
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    if (int.TryParse(noteIdString, out int noteId))
                    {
                        using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
                        {
                            MySqlCommand cmd = new MySqlCommand();
                            cmd.Connection = conn;
                            string ct = "SELECT * FROM notes WHERE id = @id";
                            cmd.CommandText = ct;
                            cmd.Parameters.AddWithValue("@id", noteId);

                            conn.Open();
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                DataTable dt = new DataTable();
                                dt.Load(reader);
                                if (dt.Rows.Count > 0)
                                {
                                    DataRow dr = dt.Rows[0];
                                    rdlDevelopment.SelectedValue = dr["development"].ToString();
                                    txtTitle.Text = dr["title"].ToString();
                                    txtKeyword.Text = dr["keyword"].ToString();
                                    txtContent.Text = HttpUtility.HtmlDecode(dr["content"].ToString());
                                }
                                ViewState["SQL_QUERY"] = ct;
                                ViewState["NOTE"] = dt;
                                ViewState["ID"] = noteId;
                            }
                        }
                    }
                }
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Logger log = new Logger(typeof(Modify).Name);
            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
            {
                try
                {
                    log.Info("Starting to modify note");
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = new MySqlCommand();
                    da.SelectCommand.CommandText = ViewState["SQL_QUERY"].ToString();
                    da.SelectCommand.Parameters.AddWithValue("@id", ViewState["ID"]);
                    da.SelectCommand.Connection = con;
                    MySqlCommandBuilder cmd = new MySqlCommandBuilder();
                    cmd.DataAdapter = da;
                    DataTable dt = (DataTable)ViewState["NOTE"];
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        dr["title"] = txtTitle.Text;
                        dr["content"] = txtContent.Text;
                        dr["keyword"] = txtKeyword.Text;
                        log.Debug($"New note info: {txtTitle.Text} - {txtContent.Text}");
                        if (fuCoverPhoto.HasFile)
                        {
                            using (Stream fs = fuCoverPhoto.PostedFile.InputStream)
                            {
                                using (BinaryReader br = new BinaryReader(fs))
                                {
                                    byte[] imgData = br.ReadBytes((Int32)fs.Length);
                                    dr["pic"] = imgData;
                                }
                            }
                        }
                        int rowsUpdated = da.Update(dt);

                        if (rowsUpdated > 0)
                        {
                            log.Info($"Note modified successfully, note ID: {dr["id"].ToString()}");
                        }
                    }
                    CacheHelper.ClearAllCache();
                }

                catch (Exception ex)
                {
                    log.Error("Failed to modify new note", ex);
                    throw;
                }
                finally
                {

                    log.Info("End of note modification method");
                    log.Shutdown();
                    Response.Redirect("Default.aspx");
                }
            }

        }
    }
}