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
    [Route("/api/cocrcorruingwords")]
    public class CoOrcorruingWordController :PageController
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public CoOrcorruingWordController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [HttpGet("{word}", Name = nameof(GetCoOrcorruingWord))]
        public IActionResult GetCoOrcorruingWord(string word)
        {
            var data = _dataService.GetCoOrcorruingWord(word);
            if (data == null) return NotFound();
            var datamodel = data.Select(x => new CoOrcorruingWordListModel()
                           
               {
                co_term = x.co_term,
                score = x.score
            }).ToList();

            return Ok(datamodel);
        }

        // catch if no word was sent with request
        [HttpGet]
        public IActionResult GetCoOrcorruingWordBadRequest()
        {
            return BadRequest();
        }
        
    }
}



    