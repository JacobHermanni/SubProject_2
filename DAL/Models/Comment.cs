using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class Comment
    {
        [Key]
        public int comment_id { get; set; }

        public int comment_score { get; set; }

        public DateTime comment_creation_date { get; set; }

        public string comment_text { get; set; }

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

        public int user_id { get; set; }

        public int post_id { get; set; }
    }
}
