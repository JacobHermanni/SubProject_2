using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace DAL.Models
{
    public class RelatedWordList
    {
        [Key]
        public string term { get; set; }

        public decimal rank { get; set; }

    }
}
