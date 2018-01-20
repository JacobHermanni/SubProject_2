using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        // not mapping - displayname is not on comment, so we only find them from user on touched 
        // comment objects without help from EF automatic mapping.
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

        public int user_id { get; set; }

        public int post_id { get; set; }
    }
}
