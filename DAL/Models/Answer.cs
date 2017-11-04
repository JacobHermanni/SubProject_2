using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class Answer 
    {
        [Key]
        public int answer_id { get; set; }

        public int post_id { get; set; }

        public int parent_Id { get; set; }
    }
}
