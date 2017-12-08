using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class FavoriteListModel
    {
        public string Url { get; set; }
        public int favorite_id { get; set; }

        public int post_id { get; set; }

        public string body { get; set; }

        public int score { get; set; }

        public string title { get; set; }

        public Nullable<int> accepted_answer_id { get; set; }

        public NoteModel note { get; set; }
    }
}
