using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class UserModel
    {
        public string Url { get; set; }

        public int user_id { get; set; }

        public string user_display_name { get; set; }

        public DateTime user_creation_date { get; set; }

        // benyttes i frontend for at undgå en uformateret string.
        public string formatted_date
        {
            get { return user_creation_date.ToString("yyyy-MM-dd"); }
            set { }
        }

        public Nullable<int> user_age { get; set; }

        public string user_location { get; set; }

        //public List<ResultModel> posts { get; set; }

        //public List<CommentModel> comments { get; set; }
    }
}
