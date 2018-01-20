using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL
{
    // bruges til oprettelse af ny favorite
    public class Favorite
    {
        [Key]
        public int favorite_id
        {
            get;
            set;
        }

        public int post_id
        {
            get;
            set;
        }
    }
}
