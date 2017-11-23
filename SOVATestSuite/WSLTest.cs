using System;
using System.Collections.Generic;
using System.Text;
using WebService;
using Xunit;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebService.Models;

namespace SOVATestSuite
{
    public class WSLTest
    {
        private const string PostsApi = "http://localhost:5001/api/posts";
        private const string NoteApi = "http://localhost:5001/api/favorite/note";
        private const string FavoriteApi = "http://localhost:5001/api/favorite";


        // Create tests of Notes
        [Fact]
        public void NoteApi_CreateNoteWithValidObject_CreatedAndObjectAndUrl()
        {
            // Create favorite to ensure a favorite exists.
            var (favorite, ingoreStatusCode) = PostData($"{FavoriteApi}/", 19);

            FavoriteModel favoriteModel = JsonConvert.DeserializeObject<FavoriteModel>(favorite);

            var data = new
            {
                favorite_id = favoriteModel.favorite_id,
                body = "testing NoteApi_CreateNoteWithValidObject_CreatedAndObjectAndUrl"
            };

            var (note, StatusCode) = PostData($"{NoteApi}", data);

            Assert.Equal(HttpStatusCode.Created, StatusCode);

            NoteModel createdNote = JsonConvert.DeserializeObject<NoteModel>(note);

            Assert.Equal(favoriteModel.favorite_id, createdNote.favorite_id);
            Assert.Equal(data.body, createdNote.body);

            // Cleanup from creating favorite and note
            DeleteData($"{NoteApi}/" + favoriteModel.favorite_id);
            DeleteData($"{FavoriteApi}/" + favoriteModel.favorite_id);
        }


        [Fact]
        public void NoteApi_CreateNoteWhereNoteAlreadyExists_Conflict()
        {
            // Create favorite to ensure a favorite exists.
            var (favorite, ignoreStatusCode) = PostData($"{FavoriteApi}/", 19);

            FavoriteModel favoriteModel = JsonConvert.DeserializeObject<FavoriteModel>(favorite);

            var data = new
            {
                favorite_id = favoriteModel.favorite_id,
                body = "testing first note NoteApi_CreateNoteWhereNoteAlreadyExists_Conflict"
            };

            var (note, StatusCode) = PostData($"{NoteApi}", data);

            NoteModel createdNote = JsonConvert.DeserializeObject<NoteModel>(note);

            // Create note with same favorite_id as created note above
            
            var (note2, StatusCode2) = PostData($"{NoteApi}", data);
            
            Assert.True(string.IsNullOrEmpty(note2));

            Assert.Equal(HttpStatusCode.Conflict, StatusCode2);

            // Cleanup from creating favorite and note
            DeleteData($"{NoteApi}/" + favoriteModel.favorite_id);
            DeleteData($"{FavoriteApi}/" + favoriteModel.favorite_id);
        }

        [Fact]
        public void NoteApi_InvalidFavoriteId_Conflict()
        {
            var data = new
            {
                favorite_id = -1,
                body = "testing NoteApi_InvalidFavoriteId_Conflict"
            };

            var (note, StatusCode) = PostData($"{NoteApi}", data);

            Assert.True(string.IsNullOrEmpty(note));

            Assert.Equal(HttpStatusCode.Conflict, StatusCode);
        }


        // Note update/put tests
        [Fact]
        public void NoteApi_PutNoteWithValidId_Ok()
        {
            // Create favorite so that we can create a note on it
            var (fav, statusCodeToIgnore) = PostData(FavoriteApi, 19);

            FavoriteModel favoriteModel = JsonConvert.DeserializeObject<FavoriteModel>(fav);

            var data = new
            {
                favorite_id = favoriteModel.favorite_id,
                body = "Created note for test of Put"
            };

            PostData(NoteApi, data);

            var data2 = new
            {
                favorite_id = favoriteModel.favorite_id,
                body = "Updated body for test"
            };

            var putStatusCode = PutData(NoteApi, data2);

            Assert.Equal(HttpStatusCode.Created, putStatusCode);

            var (result, statusCode) = GetObject($"{NoteApi}/" + favoriteModel.favorite_id);

            NoteModel noteModel = JsonConvert.DeserializeObject<NoteModel>(result);

            Assert.Equal("Updated body for test", noteModel.body);

            // cleanup
            DeleteData($"{FavoriteApi}/{noteModel.favorite_id}");
            DeleteData($"{NoteApi}/{noteModel.favorite_id}");
        }

        [Fact]
        public void NoteApit_PutNote_InvalidID_NotFound()
        {
            var data = new
            {
                favorite_id = -1,
                body = "invalid id"
            };
            var statusCode = PutData(NoteApi, data);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        // Note delete tests
        [Fact]
        public void NoteApi_DeleteNoteWithValidId_Ok()
        {

            // Create favorite so that we can create a note on it
            var (fav, statusCodeToIgnore) = PostData(FavoriteApi, 19);

            FavoriteModel favoriteModel = JsonConvert.DeserializeObject<FavoriteModel>(fav);

            var data = new
            {
                favorite_id = favoriteModel.favorite_id,
                body = "Created note for test of DeleteNote()"
            };

            PostData(NoteApi, data);

            var statusCode = DeleteData($"{NoteApi}/{favoriteModel.favorite_id}");

            Assert.Equal(HttpStatusCode.OK, statusCode);

            //clean up
            DeleteData($"{FavoriteApi}/{favoriteModel.favorite_id}");
        }

        [Fact]
        public void NoteApi_DeleteNoteWithInvalidId_NotFound()
        {
            var statusCode = DeleteData($"{NoteApi}/-1");

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }



        // Posts readonly tests
        [Fact]
        public void PostApi_GetPostWithValidId_OkAndObjectWithProperID()
        {
            var (post, statusCode) = GetObject($"{PostsApi}/71");

            Assert.Equal(HttpStatusCode.OK, statusCode);

            PostModel postModel = JsonConvert.DeserializeObject<PostModel>(post);

            Assert.Equal(71, postModel.post_id);
            Assert.Equal(PostsApi + "/71", postModel.Url);
        }

        [Fact]
        public void PostApi_GetPostWithInvalidID_NotFound()
        {
            var (post, statusCode) = GetObject($"{PostsApi}/-1");
            Assert.Equal(HttpStatusCode.NotFound, statusCode);

        }

        [Fact]
        public void PostApi_GetPostWithValidId_OkAndPostObject()
        {
            var (post, statusCode) = GetObject($"{PostsApi}/19");

            Assert.Equal(HttpStatusCode.OK, statusCode);

            PostModel postModel = JsonConvert.DeserializeObject<PostModel>(post);

            Assert.Equal(164, postModel.score);
            Assert.Equal(13, postModel.user_id);
            Assert.Equal(1, postModel.post_type_id);
            Assert.Equal(PostsApi + "/19", postModel.Url);
        }

        [Fact]
        public void PostApi_GetAnswerPostWithValidId_OkAndPostObject()
        {
            var (post, statusCode) = GetObject($"{PostsApi}/19");

            Assert.Equal(HttpStatusCode.OK, statusCode);

            PostModel postModel = JsonConvert.DeserializeObject<PostModel>(post);


            Assert.Equal(13, postModel.user_id);
            Assert.Equal("Chris Jester-Young", postModel.user_display_name);
            Assert.True(postModel.question.Answers.Count > 0); // We know that post 19 is a question post with mutliple answers.
            Assert.Equal(PostsApi + "/19", postModel.Url);
        }

        [Fact]
        public void PostApi_GetSearchResultFromString_OkAndResultListObject()
        {
            // Search for java
            var (result, StatusCode) = GetObject($"{PostsApi}" + "/search/java");

            var jObject = (JObject) JsonConvert.DeserializeObject(result);

            var resultModel = jObject["data"].ToObject<List<ResultModel>>();

            // The first result has the post_id 388242
            Assert.Equal(388242, resultModel.First().post_id);

            // The first result has a score of 4271
            Assert.Equal(4271, resultModel.First().score);

            // Url for 388242 should be http://localhost:5001/api/posts/388242
            Assert.Equal("http://localhost:5001/api/posts/388242", resultModel.First().Url);
        }


        // Helpers

        (string, HttpStatusCode) GetObject(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return (data, response.StatusCode);
        }

        (string, HttpStatusCode) PostData(string url, object content)
        {
            var client = new HttpClient();
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            var response = client.PostAsync(url, requestContent).Result;
            var data = response.Content.ReadAsStringAsync().Result;
            return (data, response.StatusCode);
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
