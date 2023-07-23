using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace noteblog
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SqlConnection conn = new SqlConnection("data source=localhost; database=noteblog; integrated security=SSPI");

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand("select * from notes", conn);
                conn.Open();
                GridView1.DataSource = cmd.ExecuteReader();
                GridView1.DataBind();
            }
        }
        protected void btnFront_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString))
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from notes where development = @development";
                cmd.Parameters.AddWithValue("@development", "F");
                conn.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    //DataTable dt = new DataTable();
                    //dt.Columns.Add("ID");
                    //dt.Columns.Add("Name");
                    //dt.Columns.Add("Price");
                    //dt.Columns.Add("DiscountedPrice");
                    //while (reader.Read())
                    //{
                    //    DataRow dr = dt.NewRow();
                    //    double discountedPrice = Convert.ToInt32(reader["unitPrice"]) * 0.9;

                    //    dr["ID"] = reader["ID"];
                    //    dr["Name"] = reader["Name"];
                    //    dr["Price"] = reader["Price"];
                    //    dr["DiscountedPrice"] = discountedPrice;
                    //    dt.Rows.Add(dr);
                    //}
                    GridView1.DataSource = reader;
                    GridView1.DataBind();
                }
            }
        }
    }
}