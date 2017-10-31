using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DAL;

namespace WebService
{
    [Route("/api/post")]
    public class PostController : Controller
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public PostController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [HttpGet()]
        public IActionResult GetPosts(int page = 0, int pageSize = 5)
        {

            var posts = _dataService.GetPosts();
            if (posts == null) return NotFound();
            return Ok(posts);
        }

        private static int GetTotalPages(int pageSize, int total)
        {
            return (int)Math.Ceiling(total / (double)pageSize);
        }

        private static void CheckPageSize(ref int pageSize)
        {
            pageSize = pageSize > 50 ? 50 : pageSize;
        }
    }
}
