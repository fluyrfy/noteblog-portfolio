using System;
using System.ComponentModel.DataAnnotations.Schema;
using MySqlX.XDevAPI.Relational;

namespace noteblog.Models
{
    public class AccessStats
    {
        public int id { get; set; }
        [Column("access_time")]
        public DateTime accessTime { get; set; }
        [Column("access_page")]
        public string accessPage { get; set; }
        [Column("ip_address")]
        public string ipAddress { get; set; }
    }
}