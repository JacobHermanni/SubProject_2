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

        public string body { get; set; }

        public int user_id { get; set; }

        public UserModel user { get; set; }

        public int post_type_id { get; set; }

        [NotMapped]
        public object PostType { get; set; }

        //public QuestionModel question { get; set; }

        //public AnswerModel answer { get; set; }

        public List<CommentModel> Comments { get; set; }
    }
}
