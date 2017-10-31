using System;
using DAL;

namespace ConsoleDev
{
    class DevProgram
    {
        static void Main(string[] args)
        {
            var sovadb = new DataService();

            var postList = sovadb.GetPosts();
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(postList[i].post_id);
            }

            Console.Read();


        }
    }
}
