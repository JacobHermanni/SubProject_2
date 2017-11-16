using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL.Models
{
    public class User
    {
        [Key]
        public int user_id { get; set; }

        public string user_display_name { get; set; }

        public DateTime user_creation_date { get; set; }

        public Nullable<int> user_age { get; set; }

        public string user_location { get; set; }

        public List<Result> posts
        {
            get
            {
                using (var db = new SOVAContext())
                {
                    var getposts = db.Post.Where(p => p.user_id == user_id).Select(x => new Result()
                    {
                        post_id = x.post_id,
                        body = x.body,
                        score = x.score
                    }).ToList();

                    if (getposts.Count() == 0) return null;

                    return getposts;
                }
            }
            set { }
        }
    }
}
