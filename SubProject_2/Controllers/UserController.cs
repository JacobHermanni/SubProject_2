using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebService.Models;

namespace WebService.Controllers
{
    [Route("/api/user")]
    public class UserController : PageController
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public UserController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetUsers))]
        public IActionResult GetUsers(int page = 0, int pageSize = 25)
        {
            CheckPageSize(ref pageSize);

            var total = _dataService.GetNumberOfUsers();
            var totalPages = GetTotalPages(pageSize, total);

            var data = _dataService.GetUsers(page, pageSize)
                .Select(x => new
                {
                    Url = Url.Link(nameof(GetUser), new { id = x.user_id }),
                    user_id = x.user_id,
                    user_display_name = x.user_display_name
                });

            var result = new
            {
                Total = total,
                Pages = totalPages,
                Page = page,
                Prev = Link(nameof(GetUsers), page, pageSize, -1, () => page > 0),
                Next = Link(nameof(GetUsers), page, pageSize, 1, () => page < totalPages - 1),
                Url = Link(nameof(GetUsers), page, pageSize),
                Data = data
            };

            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetUser))]
        public IActionResult GetUser(int id)
        {
            var user = _dataService.GetUser(id);
            if (user == null) return NotFound();

            var model = _mapper.Map<UserModel>(user);
            model.Url = Url.Link(nameof(GetUser), new { id = user.user_id });

            var returnUser = new
            {
                User = model,
                UserCommentsUrl = Url.Link(nameof(GetUserComments), new { id = id }),
                UserPostsUrl = Url.Link(nameof(GetUserPosts), new { id = id })
            };

            return Ok(returnUser);
        }

        [HttpGet("usercomments/{id}", Name = nameof(GetUserComments))]
        public IActionResult GetUserComments(int id, int page = 0, int pageSize = 5)
        {
            CheckPageSize(ref pageSize);

            var postCtrl = new PostController(_dataService, _mapper);

            var data = _dataService.GetUserComments(id, page, pageSize);
            if (data == null) return NotFound();

            var userComments = data
                .Select(x => new
                {
                    x.comment_id,
                    x.comment_score,
                    x.comment_creation_date,
                    x.comment_text,
                    postUrl = Url.Link(nameof(postCtrl.GetPost), new { id = x.post_id })
                });


            var total = _dataService.GetNumberOfUserComments();
            var totalPages = GetTotalPages(pageSize, total);

            var result = new
            {
                Total = total,
                Pages = totalPages,
                Page = page,
                Prev = Link(nameof(GetUserComments), page, pageSize, -1, () => page > 0),
                Next = Link(nameof(GetUserComments), page, pageSize, 1, () => page < totalPages - 1),
                Url = Link(nameof(GetUserComments), page, pageSize),
                UserComments = userComments
            };

            return Ok(result);
        }

        [HttpGet("userposts/{id}", Name = nameof(GetUserPosts))]
        public IActionResult GetUserPosts(int id, int page = 0, int pageSize = 5)
        {
            CheckPageSize(ref pageSize);

            var postCtrl = new PostController(_dataService, _mapper);

            var data = _dataService.GetUserPosts(id, page, pageSize);
            if (data == null) return NotFound();

            var userPosts = data
                .Select(x => new
                {
                    x.post_id,
                    x.body,
                    x.score,
                    postUrl = Url.Link(nameof(postCtrl.GetPost), new { id = x.post_id })
                });


            var total = _dataService.GetNumberOfUserPosts();
            var totalPages = GetTotalPages(pageSize, total);

            var result = new
            {
                Total = total,
                Pages = totalPages,
                Page = page,
                Prev = Link(nameof(GetUserPosts), page, pageSize, -1, () => page > 0),
                Next = Link(nameof(GetUserPosts), page, pageSize, 1, () => page < totalPages - 1),
                Url = Link(nameof(GetUserPosts), page, pageSize),
                UserPosts = userPosts
            };

            return Ok(result);
        }
    }
}