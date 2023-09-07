﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace noteblog.Models
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public byte[] avatar { get; set; }
        public string role { get; set; }
        public string verificationCode { get; set; }
        public bool isVerified { get; set; }
        [Column("created_at")]
        public DateTime createdAt { get; set; }
        [Column("updated_at")]
        public DateTime updatedAt { get; set; }
    }
}