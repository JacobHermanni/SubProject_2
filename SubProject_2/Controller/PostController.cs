using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL;

namespace WebService
{
    [Route("/api/post")]
    public class PostController : Controller
    {
        private IDataService _dataService;

        public PostController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet()]
        public IActionResult GetPosts()
        {
            var posts = _dataService.GetPosts();
            if (posts == null) return NotFound();
            return Ok(posts);
        }
    }
}
