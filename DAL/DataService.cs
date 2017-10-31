using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DAL
{

    public class DataService : IDataService
    {
        public int LatestSearchCount { get; set; }

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
            if (LatestSearchCount == null) return 0;
            return LatestSearchCount;
        }

        //public int GetNumberOfSearchresults()
        //{
        //    using (var db = new SOVAContext())
        //    {
        //        return db.SearchLists.Count();
        //    }
        //}

        public List<SearchList> GetPostsByString(string searchString, int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                //return db.SearchLists.FromSql("call search_pass({0})", searchString).
                //    OrderByDescending(x => x.score)
                //    .Skip(page * pageSize)
                //    .Take(pageSize)
                //    .ToList();

                var returnedList = db.SearchLists.FromSql("call search_pass({0})", searchString);
                LatestSearchCount = returnedList.ToList().Count;

                return returnedList.
                    OrderByDescending(x => x.score)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }
    }
}
