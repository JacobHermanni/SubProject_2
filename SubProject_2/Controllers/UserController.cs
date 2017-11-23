using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL;
using Microsoft.AspNetCore.Mvc;
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

            var postCtrl = new PostController(_dataService, _mapper);

            // Add self-referencing urls to posts.
            if (model.posts != null)
            {
                model.posts = model.posts.Select(x => new ResultModel
                {
                    Url = Url.Link(nameof(postCtrl.GetPost), new { id = x.post_id }),
                    post_id = x.post_id,
                    body = x.body,
                    score = x.score
                }).ToList();

                foreach (var resultModel in model.posts)
                {
                    var post = _dataService.GetPost(resultModel.post_id);
                    if (post.post_type_id == 1) resultModel.title = post.question.title;
                }
            }

            // Add post-referencing urls to comments.
            if (model.comments != null)
            {
                foreach (var commentModel in model.comments)
                {
                    commentModel.postUrl = Url.Link(nameof(postCtrl.GetPost), new { id = commentModel.post_id });
                }
            }

            return Ok(model);
        }
    }
}