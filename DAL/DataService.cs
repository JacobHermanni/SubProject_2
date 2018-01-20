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
        // obsolete, posts ved søgning findes i form af WeightedResult.cs og enkelte posts findes gennem getPost()
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

        // Bruges til paging af obsolete getposts.
        public int GetNumberOfPosts()
        {
            using (var db = new SOVAContext())
            {
                return db.Post.Count();
            }
        }

        // Answers til en enkelt post. Gemmer total antal af answers for pagination.
        public List<Post> GetAnswers(int postId, int page, int pageSize)
        {
            using (var db = new SOVAContext())
            {
                var totalAnswers = db.Post.Where(a => a.answer.parent_Id == postId);
                TotalAnswers = totalAnswers.Count();

                if (!totalAnswers.Any()) return null;

                return totalAnswers
                    .OrderByDescending(x => x.score)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }

        // til pagination for answers på en enkelt post. Kun 25 answers sendes op til WSL, så total count forbliver nede i DAL.
        public int GetNumberOfAnswers()
        {
            return TotalAnswers;
        }

        public int TotalAnswers { get; set; }

        public Post GetPost(int id)
        {
            using (var db = new SOVAContext())
            {
                var post = db.Post.Find(id);

                // hvis det er et svar-post så returner i stedet parent som er spørgsmål-post.
                if (post.question == null) // if (post.post_type_id == 2)  post = db.Post.Find(post.answer.parent_Id); - mere letvægt
                {
                    post = db.Post.Find(post.answer.parent_Id);
                }

                return post;
            }
        }

        // obsolete resultat fra gammel søgeprocedure
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

        // obsolete søgning fra sektion 1
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
                var results = db.Weighted_Result.FromSql("call weightedSearch({0})", searchString)
                    .OrderByDescending(x => x.score)
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Fetch title from question even if post is an answer
                foreach (var result in results)
                {
                    result.title = GetPost(result.post_id).question.title;
                }

                return results;
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
                // tjek for eksisterende note. Hvis der er en så returner null, da redigering ikke skal ske gennem Create (rest)
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

                // returner den nyoprettede note
                return GetNote(favID);
            }
        }

        public Note UpdateNote(int favID, string body)
        {
            using (var db = new SOVAContext())
            {
                // Check if note for favorite_id exists, if not return null
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
            // statuscode kræver false hvis der ikke kunne findes noget at slette og true, hvis der var noget at slette som slettes.
            return false;
        }

        public bool DeleteFavorite(int favID)
        {
            using (var db = new SOVAContext())
            {
                var existingFavorite = db.Favorite.Find(favID);

                if (existingFavorite != null)
                {
                    // skulle have benyttet ON DELETE CASCADE i note-relation i db for automatisk at slette note når favorite slettes. kunne have været implenteret her således:
                    //DeleteNote(existingFavorite);

                    db.Favorite.Remove(existingFavorite);
                    db.SaveChanges();
                    return true;
                }
            }
            // statuscode kræver false hvis der ikke kunne findes noget at slette og true, hvis der var noget at slette som slettes.
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

        // pagination
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
                //TODO: undersøg asnotracking
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

        // obsolete
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

        // pagination
        public int TotalUserComments { get; set; }

        public int GetNumberOfUserComments()
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

                // set total comments for pagination
                TotalUserComments = getComments.Count();

                var returnComments = getComments
                    .Skip(page * pageSize)
                    .Take(pageSize)
                    .ToList();

                return returnComments;
            }
        }

        // pagination
        public int TotalUserPosts { get; set; }

        public int GetNumberOfUserPosts()
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

                // set for pagination
                TotalUserPosts = getPosts.Count();
                 
                // userposts behøver ikke fremvisning af alle attributter/properties, så WeightedResult genbruges delvist.
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

        public List<CoOrcorruingWord> GetCoOrcorruingWord(string word)
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

        public List<TermNetwork> GetTermNetwork(string word)
        {

            using (var db = new SOVAContext())
            {
                var network = db.TermNetwork.FromSql("call term_network({0})", word)
                    .ToList();

                if (!network.Any())
                {
                    return null;
                }
                return network;
            }
        }

    }
}

