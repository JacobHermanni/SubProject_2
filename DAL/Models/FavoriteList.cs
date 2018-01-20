using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    // model for den type liste vi får fra proceduren i mysql: get_favorites(). Bruges i frontend hver gang favorites kaldes.
    public class FavoriteList
    {
        [Key]
        public int favorite_id { get; set; }

        public int post_id { get; set; }

        public string body { get; set; }

        public int score { get; set; }

        public string title { get; set; }

        public Nullable<int> accepted_answer_id { get; set; }

        public Note note
        {
            get
            {
                using (var db = new SOVAContext())
                {
                    return db.Note.Find(favorite_id);
                }
            }
            set { }
        }
    }
}
