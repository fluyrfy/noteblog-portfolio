using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace noteblog.Models
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

    }
}