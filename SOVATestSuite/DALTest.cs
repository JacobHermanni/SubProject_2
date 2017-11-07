using System;
using Xunit;
using DAL;

namespace SOVATestSuite
{
    public class DALTest
    {
        // Read-only Get tests of posts
        [Fact]
        public void GetPost_ValidId_ReturnsPostObjectWithProperID()
        {
            var service = new DataService();
            var post = service.GetPost(71); // we know from the databse, that 71 is a valid id.
            Assert.Equal(71, post.post_id);
        }


        // Create tests of Notes
        [Fact]
        public void CreateNote_ValidObject_ReturnsNoteObject()
        {
            var service = new DataService();

            // Create favorite to ensure a favorite exists.
            var favorite = service.CreateFavorite(19);

            var exampleBody = "this is a note test";

            var createdNote = service.CreateNote(favorite.favorite_id, exampleBody);

            Assert.Equal(favorite.favorite_id, createdNote.favorite_id);
            Assert.Equal(exampleBody, createdNote.body);
            Assert.NotNull(createdNote.created_timestamp);

            // Cleanup from creating favorite
            service.DeleteFavorite(favorite.favorite_id);
        }

        [Fact]
        public void CreateNote_ValidObject_NoteAlreadyExists_ReturnsNull()
        {
            var service = new DataService();

            // Create favorite to ensure a favorite exists.
            var favorite = service.CreateFavorite(19);

            var exampleBody = "this is a note test";

            var createdNote1 = service.CreateNote(favorite.favorite_id, exampleBody);

            // Create note with same favorite_id as created note above

            var createdNote2 = service.CreateNote(createdNote1.favorite_id, exampleBody);

            Assert.Equal(createdNote2, null);

            // Cleanup from creating favorite
            service.DeleteFavorite(createdNote1.favorite_id);
        }

        [Fact]
        public void CreateNote_InvalidFavoriteId_ReturnsNull()
        {
            var service = new DataService();

            // favorite id 1 does not exist
            var note = service.CreateNote(1, "");

            Assert.Equal(note, null);
        }

    }
}
