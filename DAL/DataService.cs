using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class DataService : IDataService
    {
        public List<Post> GetPosts()
        {
            using (var db = new SOVAContext())
            {
                var posts = db.Post.ToList();

                return posts;
            }
        }

        public int GetNumberOfPosts()
        {
            using (var db = new SOVAContext())
            {
                return db.Post.Count();
            }
        }

    }
}
