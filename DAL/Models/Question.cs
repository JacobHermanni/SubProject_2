using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;

namespace DAL.Models
{
    public class Question : PostType
    {
        [Key]
        public int question_id { get; set; }

        public Nullable<DateTime> close_date { get; set; }

        public Nullable<int> accepted_answer_id { get; set; }

        public string title { get; set; }

        public int post_id { get; set; }

        [NotMapped]
        public List<Post> Answers
        {
            get
            {
                using (var db = new SOVAContext())
                {
                    var getAnswers = db.Answer.Where(a => a.parent_Id == post_id).ToList();

                    if (getAnswers.Count == 0) return null;

                    var children = new List<Post>();

                    foreach (var answer in getAnswers)
                    {
                        children.Add(db.Post.Where(p => p.post_id == answer.post_id).First());
                    }

                    return children.OrderByDescending(c => c.score).ToList();
                }
            }
            set { }
        }
    }
}
