using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DAL.Models;
using System.Linq;

namespace DAL
{
    public class Post
    {
        [Key]
        public int post_id { get; set; }

        public int score { get; set; }

        public DateTime creation_date { get; set; }

        public string body { get; set; }

        public int user_id { get; set; }

        public User user
        {
            get
            {
                using (var db = new SOVAContext())
                {
                    return db.User.Find(user_id);
                }
            }
            set { }
        }

        public int post_type_id { get; set; }

        public Question question
        {
            get
            {
                using (var db = new SOVAContext())
                {
                    var getQuestions = db.Question.Where(q => q.post_id == post_id);
                    if (getQuestions.Count() == 0) return null;

                    var getQuestion = getQuestions.First();

                    return getQuestion;
                }
            }
            set { }
        }
        
        public List<Comment> Comments { get
            {
                using (var db = new SOVAContext())
                {
                    var getComments = db.Comment.Where(c => c.post_id == post_id)
                        .OrderBy(x => x.comment_creation_date)
                        .ToList();

                    if (getComments.Count() == 0) return null;

                    return getComments;
                }
            }
            set { } }
    }
}
