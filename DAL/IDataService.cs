using System.Collections.Generic;

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

    }
}