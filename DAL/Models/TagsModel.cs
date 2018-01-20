using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DAL.Models
{
    public class Tags
    {
        // composite key er defineret i SOVAContext.cs
        public int post_id { get; set; }
        
        public string tag { get; set; }
    }
}
