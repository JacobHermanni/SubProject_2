using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class WeightedResultModel
    {
        public string url { get; set; }

        public int post_id { get; set; }

        public string body { get; set; }

        public int score { get; set; }

        public decimal rank { get; set; }

        public int? parent_id { get; set; }

        public string title { get; set; }

        public DateTime creation_date { get; set; }

        // benyttes i frontend for at undgå en uformateret string.
        public string formatted_date
        {
            get { return creation_date.ToString("yyyy-MM-dd"); }
            set { }
        }
    }
}
