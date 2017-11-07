using System;
using DAL;

namespace ConsoleDev
{
    class DevProgram
    {
        static void Main(string[] args)
        {
            //AnswerTest();
            //QuestionTest();
            
        }

        static void QuestionTest()
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
            Console.WriteLine("------------------------------ End of comments ------------------------------");

            var children = post.question.Answers;

            foreach (var child in children)
            {
                Console.WriteLine("child post body");
                Console.WriteLine(child.body);
                Console.WriteLine();
            }


            Console.Read();

        }

        static void AnswerTest()
        {
            var sovadb = new DataService();

            var post = sovadb.GetPost(71);

            Console.WriteLine(post.answer.post_id);

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
