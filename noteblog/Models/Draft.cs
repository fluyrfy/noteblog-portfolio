using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace noteblog.Models
{
    [Table("drafts")]
    public class Draft
    {
        public int id { get; set; }

        [Column("category_id")]
        public int categoryId { get; set; }

        public string title { get; set; }
        public string keyword { get; set; }
        public string content { get; set; }
        [Column("co-author")]
        public string coAuthor { get; set; }
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