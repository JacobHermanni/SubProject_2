using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class ResultModel
    {
        public string Url { get; set; }

        public int post_id { get; set; }

        public string body { get; set; }

        public int score { get; set; }

        public string title { get; set; }
    }
}
