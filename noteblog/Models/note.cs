using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace noteblog.Models
{
    public class Note
    {
        public int id { get; set; }
        [Column("user_id")]
        public int userId { get; set; }
        [Column("category_id")]
        public int categoryId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        [Column("content_text")]
        public string contentText { get; set; }
        public string keyword { get; set; }
        [Column("published_at")]
        public DateTime publishedAt { get; set; }
        public byte[] pic { get; set; }
        public string status { get; set; }
        public int order { get; set; }
        [Column("created_at")]
        public DateTime createdAt { get; set; }
        [Column("updated_at")]
        public DateTime updatedAt { get; set; }
    }
}