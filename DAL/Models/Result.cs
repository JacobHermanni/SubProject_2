using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL
{
    public class Result
    {
        [Key]
        public int post_id { get; set; }

        public string body { get; set; }

        public int score { get; set; }

        public string title { get; set; }
    }
}
