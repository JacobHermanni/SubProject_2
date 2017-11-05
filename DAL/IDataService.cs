using System.Collections.Generic;
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
    }
}