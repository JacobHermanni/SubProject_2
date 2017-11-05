using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DAL;
using WebService.Models;



namespace WebService
{
    [Route("/api/favorite/note")]
    public class NoteController : PageController
    {

        private readonly IDataService _dataService;
        private readonly IMapper _mapper;


        public NoteController(IDataService dataService, IMapper mapper)
        {
            _dataService = dataService;
            _mapper = mapper;
        }

        [HttpGet("{favId}", Name = nameof(GetNote))]
        public IActionResult GetNote(int favId)
        {
            var note = _dataService.GetNote(favId);
            if (note == null) return NotFound();

            var model = _mapper.Map<NoteModel>(note);
            model.Url = Url.Link(nameof(GetNote), new { favId = note.favorite_id });

            return Ok(model);
        }

        //[HttpPost{id}", Name = nameof(CreateNote))]
        //public IActionResult CreateNote(int favId, string body)
        //{
        //    var note = _dataService.CreateNote(favId, body);
        //    if (note == null) return NotFound();

        //    var model = _mapper.Map<NoteModel>(note);

        //    return CreatedResult()
        //}



    }
}
