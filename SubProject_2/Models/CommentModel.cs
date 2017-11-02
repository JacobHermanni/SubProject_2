using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class CommentModel
    {
        public int comment_id { get; set; }

        public int comment_score { get; set; }

        public DateTime comment_creation_date { get; set; }

        public string comment_text { get; set; }

        public UserModel user { get; set; }

        public int user_id { get; set; }

        public int post_id { get; set; }
    }
}
