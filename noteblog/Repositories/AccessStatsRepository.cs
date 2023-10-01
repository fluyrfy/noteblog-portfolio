﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Dapper;
using Dapper.FluentMap;
using Newtonsoft.Json;
using noteblog.Models;
using noteblog.Models.Mappings;
using noteblog.Utils;

public class AccessStatsRepository
{
    private readonly IDbConnection _dbConnection;

    public AccessStatsRepository()
    {
        _dbConnection = DatabaseHelper.GetConnection();
    }

    public DataTable getVisits(string startDate, string endDate)
    {
        string query = @"
                        SELECT YEAR(access_time) AS visit_year, MONTH(access_time) AS visit_month, COUNT(*) AS visit_count 
                        FROM ( SELECT access_time, ip_address FROM access_stats 
                        WHERE access_time >= @startDate
                        AND access_time <= @endDate
                        GROUP BY ip_address, DATE(access_time) ) AS daily_ip_visits 
                        GROUP BY visit_year, visit_month
                        ";
        var result = _dbConnection.ExecuteReader(query, new { startDate, endDate });
        DataTable dt = new DataTable();
        dt.Load(result);
        return dt;
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

    public bool insert(string accessPage)
    {
        IpInfo ipInfo = new IpInfo();
        using (_dbConnection)
        {
            try
            {
                var ipAddress = AccessHelper.GetIPAddress();
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ipAddress);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                string city = ipInfo.city;
                string country = ipInfo.country;
                string region = ipInfo.region;
                string queryVisits = "INSERT INTO access_stats (access_page, ip_address) VALUES (@accessPage, @ipAddress)";
                string queryLocations = "INSERT INTO user_locations (ip_address, country, region, city) VALUES (@ipAddress, @country, @region, @city)";

                return _dbConnection.Execute(queryVisits, new { accessPage, ipAddress }) == 1 && _dbConnection.Execute(queryLocations, new { ipAddress, country, region, city }) == 1;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}