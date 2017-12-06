using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class WeightedResult
    {
        [Key]
        public int post_id { get; set; }

        public string body { get; set; }

        public int score { get; set; }

        public decimal rank { get; set; }

        public int? parent_id { get; set; }

        [NotMapped]
        public string title { get; set; }
    }
}
