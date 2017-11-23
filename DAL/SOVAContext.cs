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

        public DbSet<Result> SearchList { get; set; }

        public DbSet<Result> Result { get; set; }

        public DbSet<Question> Question { get; set; }
        
        public DbSet<User> User { get; set; }

        public DbSet<Comment> Comment { get; set; }

        public DbSet<Answer> Answer { get; set; }

        public DbSet<History> History { get; set; }

        public DbSet<Note> Note { get; set; }

        public DbSet<Favorite> Favorite { get; set; }

        public DbSet<WeightedResult> Weighted_Result { get; set; }

        public DbSet<FavoriteList> FavoriteList { get; set; }

        public DbSet<RelatedWordList> RelatedWordList { get; set; }

        public DbSet<CoOrcorruingWordList> CoOrcorruingWordList { get; set; }

       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseMySql(
                "server=localhost;" +
                "database=SOVA;" +
                "uid=root;" +
                "pwd=Arsenal05;"
            );
        }
    }
}
