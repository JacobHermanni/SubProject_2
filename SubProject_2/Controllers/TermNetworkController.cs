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
    [Route("/api/termnetwork")]
    public class TermNetWorkController : PageController
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public TermNetWorkController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [HttpGet("{word}", Name = nameof(GetTermNetwork))]
        public IActionResult GetTermNetwork(string word)
        {

            var data = _dataService.GetTermNetwork(word);
            if (data == null) return NotFound();
            var datamodel = data.Select(x => new TermNetworkModel()
            {
                termNetwork = x.termNetwork,
            });

            return Ok(datamodel);
        }

        // for at fange api kaldet med tom string
        [HttpGet]
        public IActionResult GetTermNetworkdRequest()
        {
            return BadRequest();
        }
    }
}
