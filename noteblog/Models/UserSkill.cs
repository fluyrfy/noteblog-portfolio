using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace noteblog.Models
{
    public class UserSkill
    {
        public int id { get; set; }
        [Column("user_id")]
        public Guid userId { get; set; }
        [Column("skill_id")]
        public int skillId { get; set; }
        public string name { get; set; }
        public string percent { get; set; }
    }
}