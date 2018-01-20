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

        // benyttes i frontend for at undgå en uformateret string.
        public string formatted_date
        {
            get { return history_timestamp.ToString("MM/dd/yyyy HH:mm"); }
            set { }
        }

        public string URL
        {
            get;
            set;
        }

    }
}
