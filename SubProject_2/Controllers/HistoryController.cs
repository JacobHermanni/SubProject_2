using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DAL;
using WebService.Models;

namespace WebService
{
    [Route("/api/history")]
    public class HistoryController : PageController
    {

        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public HistoryController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetHistory))]
        public IActionResult GetHistory(int page = 0, int pageSize = 5)
        {
            CheckPageSize(ref pageSize);

            var total = _dataService.GetNumberOfHistorySearches();
            var totalPages = GetTotalPages(pageSize, total);

            var data = _dataService.GetHistory(page, pageSize)
                .Select(x => new HistoryModel
                {
                URL = "http://localhost:5001/api/posts/name/" + x.search_string.Replace(" ", "%20"),
                    history_id = x.history_id,
                    search_string = x.search_string,
                    history_timestamp = x.history_timestamp
                });

            var result = new
            {
                Total = total,
                Pages = totalPages,
                Page = page,
                Prev = Link(nameof(GetHistory), page, pageSize, -1, () => page > 0),
                Next = Link(nameof(GetHistory), page, pageSize, 1, () => page < totalPages - 1),
                Url = Link(nameof(GetHistory), page, pageSize),
                Data = data
            };

            return Ok(result);
        }
    }

}