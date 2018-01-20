using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

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

        [NotMapped]
        public List<Tags> Tags
        {
            get
            {
                using (var db = new SOVAContext())
                {
                    var getTags = db.Tags.Where(t => t.post_id == post_id).ToList();

                    if (!getTags.Any()) return null;

                    return getTags;
                }
            }
            set { }
        }
    }
}
