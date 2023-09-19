using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace noteblog.Models
{
    public class Log
    {
        public int id { get; set; }
        public string level { get; set; }
        public string message { get; set; }
        public string exception { get; set; }
        [Column("created_at")]
        public DateTime createdAt { get; set; }
    }
}