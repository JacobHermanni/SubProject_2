using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Models;
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

        public int GetNumberOfWeightedSearchresults()
        {
            using (var db = new SOVAContext())
            {
                return db.Weighted_Result.Count();
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

        public List<WeightedResult> GetWeightedPostsByString(string searchString, int page, int pageSize)
        {
            if (string.IsNullOrEmpty(searchString)) return null;
            using (var db = new SOVAContext())
            {
                return db.Weighted_Result.FromSql("call weightedSearch({0})", searchString)
                    .OrderByDescending(x => x.rank)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public List<History> GetHistory(int page, int pageSize)
        {

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
                var favorite = db.Favorite.Find(favID);

                if (existingNote != null || favorite == null) return null;

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

        public Note UpdateNote(int favID, string body)
        {
            using (var db = new SOVAContext())
            {
                // Check if note for favorite_id exists
                var existingNote = GetNote(favID);

                if (existingNote == null) return null;

                var note = new Note
                {
                    favorite_id = favID,
                    body = body,
                    created_timestamp = existingNote.created_timestamp
                };

                DeleteNote(favID);

                db.Note.Add(note);

                db.SaveChanges();

                return GetNote(favID);
            }
        }

        public bool DeleteNote(int favID)
        {
            using (var db = new SOVAContext())
            {
                var existingNote = GetNote(favID);

                if (existingNote != null)
                {
                    db.Note.Remove(existingNote);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteFavorite(int favID)
        {
            using (var db = new SOVAContext())
            {
                var existingFavorite = db.Favorite.Find(favID);

                if (existingFavorite != null)
                {
                    db.Favorite.Remove(existingFavorite);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public List<FavoriteList> GetFavorites(int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                return db.FavoriteList.FromSql("call get_favorites()")
                    .OrderByDescending(x => x.favorite_id)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public int GetNumberOfFavorites()
        {
            using (var db = new SOVAContext())
            {
                return db.Favorite.Count();
            }
        }

        public Favorite CreateFavorite(int post_id)
        {
            using (var db = new SOVAContext())
            {
                // Check if post_id is valid AsNoTracking to avoid duplicate tracking of user
                var post = db.Post.AsNoTracking().Where(x => x.post_id == post_id);

                // Check if favorite for post_id already exists
                var newFav = db.Favorite.Where(x => x.post_id == post_id);
                if (newFav.Any() || !post.Any()) return null;

                var fav = new Favorite()
                {
                    post_id = post_id
                };

                db.Favorite.Add(fav);

                db.SaveChanges();

                return fav;
            }
        }

        public List<User> GetUsers(int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                return db.User
                    .OrderBy(x => x.user_id)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        public int GetNumberOfUsers()
        {
            using (var db = new SOVAContext())
            {
                return db.User.Count();
            }
        }

        public User GetUser(int id)
        {
            using (var db = new SOVAContext())
            {
                return db.User.Find(id);
            }
        }

        public int TotalUserComments { get; set; }

        public int GetNumberOfUserComments(int userId)
        {
            return TotalUserComments;
        }

        public List<Comment> GetUserComments(int userId, int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                // Ensure count is reset each api call.
                TotalUserComments = 0;

                var getComments = db.Comment.Where(p => p.user_id == userId);
                if (!getComments.Any()) return null;
                TotalUserComments = getComments.Count();

                var returnComments = getComments
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                return returnComments;
            }
        }

        public int TotalUserPosts { get; set; }

        public int GetNumberOfUserPosts(int userId)
        {
            return TotalUserPosts;
        }

        public List<WeightedResult> GetUserPosts(int userId, int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                // Ensure count is reset each api call.
                TotalUserPosts = 0;

                var getPosts = db.Post.Where(p => p.user_id == userId);
                if (!getPosts.Any()) return null;
                TotalUserPosts = getPosts.Count();

                var returnPosts = getPosts
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .Select(x => new WeightedResult()
                    {
                        post_id = x.post_id,
                        body = x.body,
                        score = x.score
                    })
                    .ToList();

                return returnPosts;
            }

        }


        public List<RelatedWordList> GetRelatedWords(string word)
        {

            using (var db = new SOVAContext())
            {
                var relatedword = db.RelatedWordList.FromSql("call findRelatedWords_tf_idf({0})", word)
                    .ToList();

                if (!relatedword.Any())
                {
                    return null;
                }
                return relatedword;
            }
        }

        public List<CoOrcorruingWordList> GetCoOrcorruingWord(string word)
        {

            using (var db = new SOVAContext())
            {
                var wordList = db.CoOrcorruingWordList.FromSql("call findCoOrcorruingWords({0})", word)
                    .ToList();

                if (!wordList.Any())
                {
                    return null;
                }
                return wordList;
            }
        }

    }
}

