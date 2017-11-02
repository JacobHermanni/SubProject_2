using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class SOVAContext : DbContext
    {
        public DbSet<Post> Post { get; set; }

        public DbSet<SearchList> SearchLists { get; set; }

        public DbSet<Question> Question { get; set; }
        
        public DbSet<User> User { get; set; }

        public DbSet<Comment> Comment { get; set; }

        public DbSet<Answer> Answer { get; set; }
       
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
