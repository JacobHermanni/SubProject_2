using System.Collections.Generic;
using System.Net.Http.Headers;
using DAL.Models;

namespace DAL
{
    public interface IDataService
    {
        List<Post> GetPosts(int page, int pageSize);

        int GetNumberOfPosts();

        Post GetPost(int id);

        List<Result> GetPostsByString(string search, int page, int pageSize);

        int GetNumberOfSearchresults();

        List<WeightedResult> GetWeightedPostsByString(string search, int page, int pageSize);

        int GetNumberOfWeightedSearchresults();

        List<History> GetHistory(int page, int pageSize);

        int GetNumberOfHistorySearches();

        Note CreateNote(int favID, string body);

        Note GetNote(int favID);

        Note UpdateNote(int favID, string body);

        bool DeleteNote(int favID);

        bool DeleteFavorite(int favID);

        List<FavoriteList> GetFavorites(int page, int pageSize);

        int GetNumberOfFavorites();

        Favorite CreateFavorite(int post_id);

        List<User> GetUsers(int page, int pageSize);

        int GetNumberOfUsers();

        User GetUser(int id);

        List<RelatedWordList> GetRelatedWords(string word);

        List<CoOrcorruingWord> GetCoOrcorruingWord(string word);

        List<Comment> GetUserComments(int userId, int page, int pageSize);

        int GetNumberOfUserComments();

        List<WeightedResult> GetUserPosts(int userId, int page, int pageSize);

        int GetNumberOfUserPosts();

        List<Post> GetAnswers(int postId, int page, int pageSize);

        int GetNumberOfAnswers();

        List<TermNetwork> GetTermNetwork(string word);
    }
}