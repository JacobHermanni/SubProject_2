using System;
using DAL;

namespace ConsoleDev
{
    class DevProgram
    {
        static void Main(string[] args)
        {
            

        }

        void PostTest()
        {
            var sovadb = new DataService();

            var post = sovadb.GetPost(19);

            Console.WriteLine(post.question.title);

            Console.WriteLine(post.user.user_display_name);

            foreach (var postComment in post.Comments)
            {
                Console.WriteLine(postComment.user.user_display_name + ":");
                Console.WriteLine();
                Console.WriteLine(postComment.comment_text);
                Console.WriteLine();
            }

            Console.Read();

        }
    }
}
