﻿using System.Configuration;
using MySql.Data.MySqlClient;

public static class DatabaseHelper
{
    private static readonly string _connectionString;

    static DatabaseHelper()
    {
        // 在靜態建構子中讀取連線字串並儲存在靜態欄位中
        _connectionString = ConfigurationManager.ConnectionStrings["Noteblog"].ConnectionString;
    }

    public static MySqlConnection GetConnection()
    {
        // 提供一個方法回傳資料庫連線物件
        return new MySqlConnection(_connectionString);
    }
}
