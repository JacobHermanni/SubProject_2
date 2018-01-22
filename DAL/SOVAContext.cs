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
        // database mapping to class models

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

        public DbSet<CoOrcorruingWord> CoOrcorruingWordList { get; set; }

        public DbSet<Tags> Tags { get; set; }

        public DbSet<TermNetwork> TermNetwork { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseMySql(
                "server=localhost;" +
<<<<<<< HEAD
                "database=sova;" +
                "uid=root;" +
                "pwd=root;"

=======
<<<<<<< HEAD
                "database=raw9;" +
                "uid=raw9;" +
                "pwd=raw9;"
=======
                "database=SOVA;" +
                "uid=root;" +
                "pwd=root;"
<<<<<<< HEAD
>>>>>>> 626da64... commit so i can pull

=======
                "pwd=theis9953;"
>>>>>>> aa593bd... commit so i can pull
<<<<<<< HEAD
>>>>>>> 78b1a23... commit so i can pull
=======
=======
>>>>>>> 1ac68a0... commit so i can pull
>>>>>>> 93c8230... commit so i can pull
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //TODO: noget med composite key - unders鴊 mere
            modelBuilder.Entity<Tags>().HasKey(t => new { t.post_id, t.tag });

        }

    }
}

