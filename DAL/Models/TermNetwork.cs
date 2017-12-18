using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAL.Models
{
    public class TermNetwork
    {
        [Key]
        public string termNetwork { get; set; }
    }
}
