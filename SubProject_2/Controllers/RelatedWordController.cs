﻿using System;
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
    [Route("/api/relatedwords")]
    public class RelatedWordController : PageController
    {
        private readonly IDataService _dataService;
        private readonly IMapper _mapper;

        public RelatedWordController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        //[HttpGet("{word}", Name = nameof(GetRelatedWords))]
        //public IActionResult GetRelatedWords(string word)
        //{



        //    var data = _dataService.GetRelatedWords(word)
        //        .Select(x => new RelatedWordListModel()
        //        {
        //        term = x.term,
        //        rank = x.rank
        //        });

        //    return Ok(data);
        //}

        [HttpGet("{word}", Name = nameof(GetRelatedWords))]
        public IActionResult GetRelatedWords(string word)
        {
            var model = _dataService.GetRelatedWords(word);
            if (model == null) return NotFound();

            var test = _mapper.Map<RelatedWordListModel>(model);
            return Ok(test);
        }



    }
}
