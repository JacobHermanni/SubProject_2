using System;
using System.Collections.Generic;
using System.Text;
using WebService;
using Xunit;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebService.Models;

namespace SOVATestSuite
{
    public class WSLTest
    {
        private const string PostsApi = "http://localhost:5001/api/posts";
        private const string NoteApi = "http://localhost:5001/api/favorite/note";


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
            service.DeleteNote(favorite.favorite_id);
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
            service.DeleteNote(favorite.favorite_id);
        }

        [Fact]
        public void CreateNote_InvalidFavoriteId_ReturnsNull()
        {
            var service = new DataService();

            // favorite id 1 does not exist
            var note = service.CreateNote(-1, "");

            Assert.Null(note);
        }


        // Note update tests
        [Fact]
        public void UpdateNote_NewBody_UpdateWithNewValues()
        {
            var service = new DataService();

            // Create favorite so that we can create a note on it
            var fav = service.CreateFavorite(19);

            var note = service.CreateNote(fav.favorite_id, "Created note for test of UpdateNote()");

            service.UpdateNote(note.favorite_id, "Updated body for test");

            note = service.GetNote(note.favorite_id);

            Assert.Equal("Updated body for test", note.body);

            // cleanup
            service.DeleteNote(note.favorite_id);
            service.DeleteFavorite(fav.favorite_id);
        }

        [Fact]
        public void UpdateNote_InvalidID_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.UpdateNote(-1, "Update");
            Assert.Null(result);
        }

        // Note delete tests
        [Fact]
        public void DeleteNote_ValidId_RemoveTheNote()
        {
            var service = new DataService();

            // Create favorite so that we can create a note on it
            var fav = service.CreateFavorite(19);

            var note = service.CreateNote(fav.favorite_id, "Created note for test of DeleteNote()");

            var result = service.DeleteNote(note.favorite_id);
            Assert.True(result);

            // Cleanup
            service.DeleteFavorite(fav.favorite_id);
        }

        [Fact]
        public void DeleteNote_InvalidId_ReturnsFalse()
        {
            var service = new DataService();
            var result = service.DeleteNote(-1);
            Assert.False(result);
        }


        // Posts readonly tests
        [Fact]
        public void GetPost_ValidId_ReturnsPostObjectWithProperID()
        {
            var service = new DataService();
            var post = service.GetPost(71); // we know from the databse, that 71 is a valid id.
            Assert.Equal(71, post.post_id);
        }

        [Fact]
        public void GetPost_invalidID_ReturnNull()
        {
            var service = new DataService();
            var post = service.GetPost(-1);
            Assert.Null(post);
        }

        [Fact]
        public void GetPost_ValidId_ReturnsPostObject()
        {
            var service = new DataService();
            var post = service.GetPost(19);
            Assert.Equal(164, post.score);
            Assert.Equal(13, post.user_id);
            Assert.Equal(1, post.post_type_id);

        }

        [Fact]
        public void GetPost_ValidId_ReturnsPostObjectWithChildren()
        {
            var (post, statusCode) = GetObject($"{PostsApi}/19");

            Assert.Equal(HttpStatusCode.OK, statusCode);

            PostModel postModel = null;
            post.JsonConvert.PopulateObject(postModel);
                

            Assert.Equal(13, post.user_id);
            Assert.Equal(35, post.user.user_age);
            Assert.Equal("Chris Jester-Young", post.user.user_display_name);
            Assert.True(post.question.Answers.Count > 0); // We know that post 19 is a question post with mutliple answers.
        }

        // Helpers

        (JArray, HttpStatusCode) GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JArray)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        (JObject, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return ((JObject)JsonConvert.DeserializeObject(data), response.StatusCode);
        }

        HttpStatusCode PutData(string url, object content)
        {
            var client = new HttpClient();
            var response = client.PutAsync(
                url,
                new StringContent(
                    JsonConvert.SerializeObject(content),
                    Encoding.UTF8,
                    "application/json")).Result;
            return response.StatusCode;
        }

        HttpStatusCode DeleteData(string url)
        {
            var client = new HttpClient();
            var response = client.DeleteAsync(url).Result;
            return response.StatusCode;
        }
    }
}
