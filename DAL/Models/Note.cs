using System;
using System.ComponentModel.DataAnnotations;


namespace DAL
{
    public class Note
    {
        [Key]
        public int note_id
        {
            get;
            set;
        }

        public int favorite_id
        {
            get;
            set;
        }

        public Nullable<DateTime> created_timestamp
        {
            get;
            set;
        }

        public string body
        {
            get;
            set;
        }



    }
}
