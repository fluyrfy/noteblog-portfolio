using System;
using System.Data;
using System.Linq;
using System.Net;
using Dapper;
using Newtonsoft.Json;
using noteblog.Models;
using noteblog.Utils;

public class AccessStatsRepository
{
  private readonly IDbConnection _dbConnection;

  public AccessStatsRepository()
  {
    _dbConnection = DatabaseHelper.GetConnection();
  }

  public bool isIpRecordExistToday(string ipAddress)
  {
    string query = "SELECT EXISTS(SELECT 1 FROM access_stats WHERE ip_address = @ipAddress AND DATE(access_time) = CURDATE()) as 'exists'";
    return _dbConnection.QuerySingleOrDefault<bool>(query, new { ipAddress });
  }

  public object getNotes(string startDate, string endDate)
  {
    string query = @"
      SELECT DATE_FORMAT(created_at, '%Y-%m') AS month, COUNT(*) AS notes
      FROM notes
      WHERE created_at BETWEEN @startDate AND @endDate
      GROUP BY month
      ORDER BY month;
    ";
    var result = _dbConnection.ExecuteReader(query, new { startDate, endDate });
    DataTable dt = new DataTable();
    dt.Load(result);
    var month = dt.AsEnumerable().Select(row => row.Field<string>("month")).ToArray();
    var notes = dt.AsEnumerable().Select(row => row.Field<long>("notes")).ToArray();
    var response = new
    {
      filter = month,
      results = notes
    };
    return response;
  }

  public object getVisits(string startDate, string endDate)
  {
    string query = @"
      SELECT DATE_FORMAT(access_time, '%Y-%m') AS month, COUNT(*) AS visits
      FROM access_stats
      WHERE access_time BETWEEN @startDate AND @endDate
      GROUP BY month
      ORDER BY month;
    ";
    var result = _dbConnection.ExecuteReader(query, new { startDate, endDate });
    DataTable dt = new DataTable();
    dt.Load(result);
    var month = dt.AsEnumerable().Select(row => row.Field<string>("month")).ToArray();
    var visits = dt.AsEnumerable().Select(row => row.Field<long>("visits")).ToArray();
    var response = new
    {
      filter = month,
      results = visits
    };
    return response;
  }
  public object getRegions(string startDate, string endDate)
  {
    string query = @"
      SELECT DATE_FORMAT(timestamp, '%Y-%m') AS month, region, COUNT(*) AS data FROM user_locations WHERE timestamp BETWEEN @startDate AND @endDate GROUP BY country;
    ";
    var result = _dbConnection.ExecuteReader(query, new { startDate, endDate });
    DataTable dt = new DataTable();
    dt.Load(result);
    var region = dt.AsEnumerable().Select(row => row.Field<string>("region")).ToArray();
    var data = dt.AsEnumerable().Select(row => row.Field<long>("data")).ToArray();
    var response = new
    {
      filter = region,
      results = data
    };
    return response;
  }

  public DataTable getLocations(string startDate, string endDate)
  {
    string query = @"
                        SELECT 
                          region,
                          COUNT(*) AS uv
                        FROM
                        (
                          SELECT 
                            ip_address, 
                            region,
                            timestamp
                          FROM user_locations 
                          WHERE timestamp >= @startDate
                            AND timestamp < @endDate
                          GROUP BY ip_address, timestamp
                        ) AS daily_ip
                        GROUP BY region
                        ";
    var result = _dbConnection.ExecuteReader(query, new { startDate, endDate });
    DataTable dt = new DataTable();
    dt.Load(result);
    return dt;
  }

  public string insert(string accessPage)
  {
    IpInfo ipInfo = new IpInfo();
    var ipAddress = AccessHelper.GetIPAddress();
    using (_dbConnection)
    {
      try
      {
        if (string.IsNullOrEmpty(ipAddress) || ipAddress == "127.0.0.1" || isIpRecordExistToday(ipAddress))
        {
          return "ip is incorrect or already exists.";
        }
        string info = new WebClient().DownloadString("https://ipinfo.io/" + ipAddress);
        ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
        string city = ipInfo.city ?? "unknown";
        string country = ipInfo.country ?? "unknown";
        string region = ipInfo.region ?? "unknown";
        string queryVisits = "INSERT INTO access_stats (access_page, ip_address) VALUES (@accessPage, @ipAddress)";
        string queryLocations = "INSERT INTO user_locations (ip_address, country, region, city) VALUES (@ipAddress, @country, @region, @city)";
        _dbConnection.Execute(queryVisits, new { accessPage, ipAddress });
        _dbConnection.Execute(queryLocations, new { ipAddress, country, region, city });
        return $"ip: {ipAddress}, info: {info}";
      }
      catch (Exception ex)
      {
        return $"ip: {ipAddress}, ex: {ex.ToString()}";
        throw;
      }
    }
  }
}
