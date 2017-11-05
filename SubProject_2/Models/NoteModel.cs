using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class NoteModel
    {
        public string Url { get; set; }

        public int favorite_id { get; set; }

        public DateTime created_timestamp { get; set; }

        public string body { get; set; }
    }
}
