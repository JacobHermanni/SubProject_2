using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class QuestionModel
    {
        public int question_id { get; set; }

        public Nullable<DateTime> close_date { get; set; }

        public Nullable<int> accepted_answer_id { get; set; }

        public string title { get; set; }

        public int post_id { get; set; }

        public List<PostModel> ChildrenPosts { get; set; }
    }
}
