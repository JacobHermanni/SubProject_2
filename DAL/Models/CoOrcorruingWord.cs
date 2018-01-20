using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace DAL.Models
{
    public class CoOrcorruingWord
    {
        [Key]
        public string co_term { get; set; }

        public int score { get; set; }
    }
}
