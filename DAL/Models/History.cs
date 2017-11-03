using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL
{
    public class History
    {
        [Key]
        public int history_id { get; set; }

        public string search_string
        {
            get;
            set;
        }

        public DateTime history_timestamp
        {
            get;
            set;
        }

    }
}
