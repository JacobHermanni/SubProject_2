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

        public Nullable<int> user_age { get; set; }

        public string user_location { get; set; }

        public List<ResultModel> posts { get; set; }

        //public List<ResultModel> comments { get; set; }
    }
}
