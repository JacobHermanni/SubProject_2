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
    [Route("/api/favorite")]
    public class FavoriteController : PageController
    {

        private readonly IDataService _dataService;
        private readonly IMapper _mapper;


        public FavoriteController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [HttpDelete("{favID}", Name = nameof(DeleteFavorite))]
        public IActionResult DeleteFavorite(int favID)
        {
            var note = _dataService.DeleteFavorite(favID);

            if (note) return Ok();
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetFavorites(int page = 0, int pageSize = 5)
        {
            CheckPageSize(ref pageSize);

            var favorites = _dataService.GetFavorites(page, pageSize)
                  .Select(x => new FavoriteListModel()
                  {
                      post_id = x.post_id,
                      Url = Url.Link(nameof(GetPost), new { id = x.post_id }),
                      favorite_id = x.favorite_id,
                      title = x.title,
                      body = x.body,
                      score = x.score,
                      accepted_answer_id = x.accepted_answer_id,
                      note = _mapper.Map<NoteModel>(x.note)
                  });
            
            var total = _dataService.GetNumberOfFavorites();
            var totalPages = GetTotalPages(pageSize, total);

            var result = new
            {
                Total = total,
                Pages = totalPages,
                Page = page,
                Prev = Link(nameof(GetFavorites), page, pageSize, -1, () => page > 0),
                Next = Link(nameof(GetFavorites), page, pageSize, 1, () => page < totalPages - 1),
                Url = Link(nameof(GetFavorites), page, pageSize),
                Data = favorites
            };


            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateFavorite([FromBody]int post_id)
        {
            var fav = _dataService.CreateFavorite(post_id);

            if (fav == null) return StatusCode(409);
            return Created("http://localhost:5001/api/favorite/" + fav.favorite_id, fav);
        }

        public IActionResult GetPost(int id)
        {
            var post = _dataService.GetPost(id);
            if (post == null) return NotFound();

            var model = _mapper.Map<PostModel>(post);
            model.Url = Url.Link(nameof(GetPost), new { id = post.post_id });

            return Ok(model);
        }
    }
}
