using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL
{

    public class DataService : IDataService
    {
        public List<Post> GetPosts(int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                return db.Post
                    .OrderBy(x => x.post_id)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public int GetNumberOfPosts()
        {
            using (var db = new SOVAContext())
            {
                return db.Post.Count();
            }
        }

        public Post GetPost(int id)
        {
            using (var db = new SOVAContext())
            {
                return db.Post.Find(id);
            }
        }

        public int GetNumberOfSearchresults()
        {
            using (var db = new SOVAContext())
            {
                return db.Result.Count();
            }
        }

        public List<Result> GetPostsByString(string searchString, int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                return db.SearchList.FromSql("call search_pass({0})", searchString)
                         .OrderByDescending(x => x.score)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public List<History> GetHistory(int page, int pageSize){

            using (var db = new SOVAContext())
            {
                return db.History
                         .OrderByDescending(x => x.history_timestamp)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            } 
        }

        public int GetNumberOfHistorySearches()
        {
            using (var db = new SOVAContext())
            {
                return db.History.Count();
            }
        }

        public Note GetNote(int favID)
        {
            using (var db = new SOVAContext())
            {
                return db.Note.Find(favID);
            } 
        }

        public Note CreateNote(int favID, string body)
        {
            using (var db = new SOVAContext())
            {
                var existingNote = GetNote(favID);

                if (existingNote != null) return null;

                var note = new Note
                {
                    favorite_id = favID,
                    body = body,
                    created_timestamp = DateTime.Now
                };

                db.Note.Add(note);

                db.SaveChanges();

                return GetNote(favID);
            }
        }



    }
}
