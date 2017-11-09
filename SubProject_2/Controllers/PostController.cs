using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DAL;
using DAL.Models;
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

            // Add self-referencing urls to posts in the case of the returned post being a question.
            if (model.question != null)
            {
                foreach (var answer in model.question.Answers)
                {
                    answer.Url = Url.Link(nameof(GetPost), new { id = answer.post_id });
                }
            }

            return Ok(model);
        }

        [HttpGet("search/{searchstring}", Name = nameof(GetPostsByString))]
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
    }
}
