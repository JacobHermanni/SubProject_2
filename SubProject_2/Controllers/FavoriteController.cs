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

        [HttpGet(Name = nameof(GetFavorites))]
        public IActionResult GetFavorites(int page = 0, int pageSize = 25)
        {
            // instantiates for access to link creation methods
            var postCtrl = new PostController(_dataService, _mapper);
            var noteCtrl = new NoteController(_dataService, _mapper);


            var favorites = _dataService.GetFavorites(page, pageSize)
                  .Select(x => new FavoriteListModel()
                  {
                      post_id = x.post_id,
                      Url = Url.Link(nameof(postCtrl.GetPost), new { id = x.post_id }),
                      favorite_id = x.favorite_id,
                      title = x.title,
                      body = x.body,
                      score = x.score,
                      accepted_answer_id = x.accepted_answer_id,
                      note = _mapper.Map<NoteModel>(x.note)
                  }).ToList();

            // Set urls for all notes
            foreach (var favoriteListModel in favorites)
            {
                if (favoriteListModel.note != null)
                {
                    favoriteListModel.note.Url =
                        Url.Link(nameof(noteCtrl.GetNote),
                        new { favId = favoriteListModel.favorite_id });
                }
            }

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

            // bør have statuskode 404 not found, hvis ingen favorites eksisterer
            return Ok(result);
        }



        [HttpPost]
        public IActionResult CreateFavorite([FromBody]int postid)
        {
            var fav = _dataService.CreateFavorite(postid);

            if (fav == null) return StatusCode(409);
            return Created(Url.Link(nameof(GetFavorites), null), fav);
        }

        // obsolete
        public class FavoriteDataObject
        {
            public int postId { get; set; }
        }
    }


}
