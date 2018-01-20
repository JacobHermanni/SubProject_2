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

        // benyttes i frontend for at undgå en uformateret string.
        public string formatted_date
        {
            get { return comment_creation_date.ToString("MM/dd/yyyy HH:mm"); }
            set { }
        }

        public string comment_text { get; set; }

        public string user_display_name { get; set; }

        public int user_id { get; set; }

        public string userUrl { get; set; }

        public int post_id { get; set; }

        public string postUrl { get; set; }
    }
}
