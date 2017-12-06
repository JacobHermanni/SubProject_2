using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DAL;
using DAL.Models;
using WebService.Controllers;
using WebService.Models;

namespace WebService
{
    [Route("/api/posts")]
    public class PostController : PageController
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public PostController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }


        // Obsolete
        [HttpGet(Name = nameof(GetPosts))]
        public IActionResult GetPosts(int page = 0, int pageSize = 25)
        {
            CheckPageSize(ref pageSize);

            var total = _dataService.GetNumberOfPosts();
            var totalPages = GetTotalPages(pageSize, total);

            var data = _dataService.GetPosts(page, pageSize)
                .Select(x => new PostModel
                {
                    Url = Url.Link(nameof(GetPost), new { id = x.post_id }),
                    post_id = x.post_id
                });

            var result = new
            {
                Total = total,
                Pages = totalPages,
                Page = page,
                Prev = Link(nameof(GetPosts), page, pageSize, -1, () => page > 0),
                Next = Link(nameof(GetPosts), page, pageSize, 1, () => page < totalPages - 1),
                Url = Link(nameof(GetPosts), page, pageSize),
                Data = data
            };

            return Ok(result);
        }

        [HttpGet("{id}", Name = nameof(GetPost))]
        public IActionResult GetPost(int id)
        {
            var post = _dataService.GetPost(id);
            if (post == null) return NotFound();

            var model = _mapper.Map<PostModel>(post);
            model.Url = Url.Link(nameof(GetPost), new { id = post.post_id });
            if(model.answer != null) model.answer.parentUrl = Url.Link(nameof(GetPost), new { id = model.answer.parent_Id });

            // Adding self-referencing urls to posts in the case of the returned post being a question.
            if (model.question != null)
            {
                foreach (var answerPost in model.question.Answers)
                {
                    answerPost.Url = Url.Link(nameof(GetPost), new { id = answerPost.post_id });
                    answerPost.answer.parentUrl = Url.Link(nameof(GetPost), new { id = answerPost.answer.parent_Id });
                }
                
                // Adding self-referencing urls to users in posts and comments.
                model = InsertUserUrls(model);
            }

            return Ok(model);
        }

        [HttpGet("search/{searchstring}", Name = nameof(GetWeightedPostsByString))]
        public IActionResult GetWeightedPostsByString(string searchstring, int page = 0, int pageSize = 25)
        {
            if (string.IsNullOrEmpty(searchstring)) return BadRequest();

            CheckPageSize(ref pageSize);

            var data = _dataService.GetWeightedPostsByString(searchstring, page, pageSize)
                .Select(x => new WeightedResultModel()
                {
                    body = x.body,
                    score = x.score,
                    url = Url.Link(nameof(GetPost), new { id = x.post_id }),
                    post_id = x.post_id,
                    parent_id = x.parent_id,
                    title = x.title
                    
                }).ToList();

            if (!data.Any()) return NotFound();

           // Set answer posts urls to reference to parent post
            foreach (var model in data)
            {
                if (model.parent_id != null) model.url = Url.Link(nameof(GetPost), new {id = model.parent_id});
            }

            var total = _dataService.GetNumberOfWeightedSearchresults();
            var totalPages = GetTotalPages(pageSize, total);

            var result = new
            {
                Total = total,
                Pages = totalPages,
                Page = page,
                Prev = Link(nameof(GetWeightedPostsByString), page, pageSize, -1, () => page > 0),
                Next = Link(nameof(GetWeightedPostsByString), page, pageSize, 1, () => page < totalPages - 1),
                Url = Link(nameof(GetWeightedPostsByString), page, pageSize),
                Data = data
            };

            return Ok(result);
        }

        // Obsolete
        //[HttpGet("search/{searchstring}", Name = nameof(GetPostsByString))]
        public IActionResult GetPostsByString(string searchstring, int page = 0, int pageSize = 25)
        {
            CheckPageSize(ref pageSize);

            var data = _dataService.GetPostsByString(searchstring, page, pageSize)
                .Select(x => new ResultModel()
                {
                    title = x.title,
                    body = x.body,
                    score = x.score,
                    Url = Url.Link(nameof(GetPost), new { id = x.post_id }),
                    post_id = x.post_id
                });

            var total = _dataService.GetNumberOfSearchresults();
            var totalPages = GetTotalPages(pageSize, total);

            var result = new
            {
                Total = total,
                Pages = totalPages,
                Page = page,
                Prev = Link(nameof(GetPostsByString), page, pageSize, -1, () => page > 0),
                Next = Link(nameof(GetPostsByString), page, pageSize, 1, () => page < totalPages - 1),
                Url = Link(nameof(GetPostsByString), page, pageSize),
                Data = data
            };

            return Ok(result);
        }

        private PostModel InsertUserUrls(PostModel model)
        {
            var userCtrl = new UserController(_dataService, _mapper);

            // Set post user link.
            model.userUrl = Url.Link(nameof(userCtrl.GetUser), new {id = model.user_id});

            // Set post comments users links and comments post links
            foreach (var modelComment in model.Comments)
            {
                modelComment.userUrl = Url.Link(nameof(userCtrl.GetUser), new { id = modelComment.user_id });
                modelComment.postUrl = Url.Link(nameof(GetPost), new { id = modelComment.post_id });
            }

            // Set answers user links.
            foreach (var answer in model.question.Answers)
            {
                answer.userUrl = Url.Link(nameof(userCtrl.GetUser), new { id = answer.user_id });

                // Set comments users links and comments post links
                foreach (var answerComment in answer.Comments)
                {
                    answerComment.userUrl = Url.Link(nameof(userCtrl.GetUser), new { id = answerComment.user_id });
                    answerComment.postUrl = Url.Link(nameof(GetPost), new { id = answerComment.post_id });
                }
            }

            return model;
        }
    }
}
