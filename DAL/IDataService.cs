using System.Collections.Generic;

namespace DAL
{
    public interface IDataService
    {
        List<Post> GetPosts();

        int GetNumberOfPosts();

    }
}