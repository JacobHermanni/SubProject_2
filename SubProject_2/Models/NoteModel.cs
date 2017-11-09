using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class NoteModel
    {
        private string url { get; set; }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public int favorite_id { get; set; }

        public DateTime created_timestamp { get; set; }

        public string body { get; set; }
    }
}
