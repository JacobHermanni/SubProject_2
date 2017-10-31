using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class Question
    {
        [Key]
        public int question_id { get; set; }

        public Nullable<DateTime> close_date { get; set; }

        public Nullable<int> accepted_answer_id { get; set; }

        public string title { get; set; }

        public int post_id { get; set; }

        
    }
}
