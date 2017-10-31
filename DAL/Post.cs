using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class Post
    {
        [Key]
        public int post_id { get; set; }
    }
}
