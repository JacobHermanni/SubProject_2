using System;
using DAL;

namespace ConsoleDev
{
    class DevProgram
    {
        static void Main(string[] args)
        {
            var sovadb = new DataService();

            var returnedPrc = sovadb.GetPostsByString("how to", 0, 10);
            Console.WriteLine(returnedPrc);

            foreach (var searchList in returnedPrc)
            {
                Console.WriteLine();
                Console.WriteLine($"{searchList.post_id}, {searchList.score}");
                Console.WriteLine();
            }

            Console.Read();


        }
    }
}
