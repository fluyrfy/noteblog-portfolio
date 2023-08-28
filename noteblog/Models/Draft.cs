using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Dapper;

namespace noteblog.Models
{
    public class Draft
    {
        public int id { get; set; }
        public string title { get; set; }
        public string keyword { get; set; }
        public string development { get; set; }
        public string content { get; set; }
        public byte[] pic { get; set; }
        [Column("user_id")]
        public int userId { get; set; }
        [Column("note_id")]
        public int noteId { get; set; }
        [Column("created_at")]
        public DateTime createdAt { get; set; }
        [Column("updated_at")]
        public DateTime updatedAt { get; set; }

    }
}