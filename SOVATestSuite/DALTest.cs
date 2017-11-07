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

<<<<<<< HEAD
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

=======




























































        [Fact]
        public void UpdateNote_NewBody_UpdateWithNewValues()
        {
            var service = new DataService();

            // Create favorite so that we can create a note on it
            service.CreateFavorite(19);

            var note = service.CreateNote(1, "Created note for test of UpdateNote()");

            var result = service.UpdateNote(note.favorite_id, "Updated body for test");

            note = service.GetNote(note.favorite_id);

            Assert.Equal("Updated body for test", note.body);
            Assert.Equal("childrencontent", note.child.body);

            // cleanup
            service.DeleteNote(note.favorite_id);
        }

        [Fact]
        public void UpdateNote_InvalidID_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.UpdateNote(-1, "Update");
            Assert.Null(result);
        }

        [Fact]
        public void DeleteNote_ValidId_RemoveTheNote()
        {
            var service = new DataService();

            // Create favorite so that we can create a note on it
            service.CreateFavorite(19);

            var note = service.CreateNote(1, "Created note for test of DeleteNote()");

            var result = service.DeleteNote(note.favorite_id);
            Assert.True(result);
        }

        [Fact]
        public void DeleteNote_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.DeleteNote(-1);
            Assert.False(result);
        }










>>>>>>> 78f4ca8... tests for update and delete Note
    }
}
