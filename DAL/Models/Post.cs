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

        // not mapping - displayname is not on post, so we only find them from user on touched 
        // post objects without help from EF automatic mapping.
        [NotMapped]
        public string user_display_name
        {
            get
            {
                using (var db = new SOVAContext())
                {
                    return db.User.Find(user_id).user_display_name;
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
                    if (!getQuestions.Any()) return null;

                    // first() fordi Iqueryable er en liste
                    return getQuestions.First();
                }
            }
            set { }
        }

        public Answer answer
        {
            get
            {
                using (var db = new SOVAContext())
                {
                    var getAnswers = db.Answer.Where(a => a.post_id == post_id);
                    if (!getAnswers.Any()) return null;

                    return getAnswers.First();
                }
            }
            set { }
        }

        public List<Comment> Comments
        {
            get
            {
                using (var db = new SOVAContext())
                {
                    var getComments = db.Comment.Where(c => c.post_id == post_id)
                        .OrderBy(x => x.comment_creation_date)
                        .ToList();

                    if (!getComments.Any()) return null;

                    return getComments;
                }
            }
            set { }
        }
    }
}
