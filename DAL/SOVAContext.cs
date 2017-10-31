using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class SOVAContext : DbContext
    {
        public DbSet<Post> Post { get; set; }

        public DbSet<SearchList> SearchLists { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseMySql(
                "server=localhost;" +
                "database=SOVA;" +
                "uid=root;" +
                "pwd=root;"
            );

        }
    }
}
