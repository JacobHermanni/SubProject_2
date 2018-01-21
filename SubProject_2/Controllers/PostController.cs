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

            // mapper og indsætter url
            var model = _mapper.Map<PostModel>(post);
            model.Url = Url.Link(nameof(GetPost), new { id = post.post_id });

            var userCtrl = new UserController(_dataService, _mapper);

            // laver url til metoden i denne klasse 'GetAnswers' gennem Question objektet på post-modellen.
            model.question.answersUrl = Url.Link(nameof(GetAnswers), new { id = post.post_id });

            // Add referencing links to users and posts on comments
            if (model.Comments != null)
            {
                foreach (var comment in model.Comments)
                {
                    comment.userUrl = Url.Link(nameof(userCtrl.GetUser), new {id = comment.user_id});
                    comment.postUrl = Url.Link(nameof(GetPost), new {id = comment.post_id});
                }
            }

            return Ok(model);
        }

        [HttpGet("search/{searchstring}", Name = nameof(GetWeightedPostsByString))]
        public IActionResult GetWeightedPostsByString(string searchstring, int page = 0, int pageSize = 25)
        {
            if (string.IsNullOrEmpty(searchstring)) return BadRequest();

            // PageController metode der tjekker at der maks vises 50 per side.
            CheckPageSize(ref pageSize);

            var data = _dataService.GetWeightedPostsByString(searchstring, page, pageSize)
                .Select(x => new WeightedResultModel()
                {
                    body = x.body,
                    score = x.score,
                    url = Url.Link(nameof(GetPost), new { id = x.post_id }),
                    post_id = x.post_id,
                    parent_id = x.parent_id,
                    title = x.title,
                    creation_date = x.creation_date

                }).ToList();

            // statuskode for intet resultat
            if (!data.Any()) return NotFound();

            // Set answer posts urls to reference to parent post
            foreach (var model in data)
            {
                if (model.parent_id != null) model.url = Url.Link(nameof(GetPost), new { id = model.parent_id });
            }

            // dataservice laver count på det seneste søgeresultat som er gemt i et table i db. Yderligere caching benyttes ikke, men det er klart.
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

        [HttpGet("answers/{id}", Name = nameof(GetAnswers))]
        public IActionResult GetAnswers(int id, int page = 0, int pageSize = 5)
        {
            CheckPageSize(ref pageSize);

            var data = _dataService.GetAnswers(id, page, pageSize);
            if (data == null) return NotFound();

            var userCtrl = new UserController(_dataService, _mapper);

            // anynomt objekt frem for standard postmodel. 
            var answers = data
                .Select(x => new
                {
                    x.post_id,
                    x.body,
                    x.score,
                    creationDate = x.creation_date.ToString("MM/dd/yyyy HH:mm"),
                    x.user_display_name,
                    x.user_id,
                    userUrl = Url.Link(nameof(userCtrl.GetUser), new { id = x.user_id }),
                    // mapper liste af DAL comments til WSL commentmodels.
                    Comments = _mapper.Map<List<Comment>, List<CommentModel>>(x.Comments)
                }).ToList();

            foreach (var answer in answers)
            {
                if (answer.Comments != null)
                {
                    foreach (var answerComment in answer.Comments)
                    {
                        answerComment.userUrl = Url.Link(nameof(userCtrl.GetUser), new { id = answer.user_id });
                        answerComment.postUrl = Url.Link(nameof(GetPost), new { id = answer.post_id });
                    }
                }
            }


            var total = _dataService.GetNumberOfAnswers();
            var totalPages = GetTotalPages(pageSize, total);

            var result = new
            {
                Total = total,
                Pages = totalPages,
                Page = page,
                Prev = Link(nameof(GetAnswers), page, pageSize, -1, () => page > 0),
                Next = Link(nameof(GetAnswers), page, pageSize, 1, () => page < totalPages - 1),
                Url = Link(nameof(GetAnswers), page, pageSize),
                Answers = answers
            };

            return Ok(result);
        }

    }
}
