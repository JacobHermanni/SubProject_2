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


        // Disse to hentes fra UserController.cs

        //public List<Result> posts
        
        //public List<Comment> comments
        
    }
}
