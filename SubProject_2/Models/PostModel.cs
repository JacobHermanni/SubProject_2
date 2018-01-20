using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class PostModel
    {
        
        public string Url { get; set; }

        public int post_id { get; set; }

        public int score { get; set; }

        public DateTime creation_date { get; set; }

        // benyttes i frontend for at undgå en uformateret string.
        public string formatted_date
        {
            get { return creation_date.ToString("yyyy-MM-dd"); }
            set { }
        }

        public string body { get; set; }

        public int user_id { get; set; }

        public string user_display_name { get; set; }

        public string userUrl { get; set; }

        public int post_type_id { get; set; }

        public QuestionModel question { get; set; }

        public AnswerModel answer { get; set; }

        public List<CommentModel> Comments { get; set; }

    }
}
