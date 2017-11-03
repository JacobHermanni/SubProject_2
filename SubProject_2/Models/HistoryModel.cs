using System;



namespace WebService.Models
{
    public class HistoryModel
    {
        
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

        public string URL
        {
            get;
            set;
        }

    }
}
