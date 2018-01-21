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
        public IActionResult GetHistory(int page = 0, int pageSize = 25)
        {
            CheckPageSize(ref pageSize);

            var postCtrl = new PostController(_dataService, _mapper);

            var total = _dataService.GetNumberOfHistorySearches();
            var totalPages = GetTotalPages(pageSize, total);

            var data = _dataService.GetHistory(page, pageSize)
                .Select(x => new HistoryModel
                {
                    // postcontroller link til søgning på søgefrasen via nameof
                    URL = Url.Link(nameof(postCtrl.GetWeightedPostsByString), new { searchstring = x.search_string }),
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

            // bør have statuskode 404 not found, hvis ingen favorites eksisterer
            return Ok(result);
        }
    }

}