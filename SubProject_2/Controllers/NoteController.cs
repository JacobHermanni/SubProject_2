using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            // mapper enkelt objekt og derefter indsætter enkel url
            var model = _mapper.Map<NoteModel>(note);
            model.Url = Url.Link(nameof(GetNote), new { favId = note.favorite_id });

            return Ok(model);
        }

        [HttpPost]
        public IActionResult CreateNote([FromBody]Note sentNote)
        {
            var note = _dataService.CreateNote(sentNote.favorite_id, sentNote.body);

            if (note == null) return StatusCode(409);
            return Created(Url.Link(nameof(GetNote), new {favId = note.favorite_id }), note);
        }

        [HttpPut]
        // frombody henter ikke variablen fra url, men fra json body
        public IActionResult UpdateNote([FromBody]Note sentNote)
        {
            var note = _dataService.UpdateNote(sentNote.favorite_id, sentNote.body);

            if (note == null) return NotFound();
            return Created(Url.Link(nameof(GetNote), new {favId = note.favorite_id}), note);
        }

        [HttpDelete("{favID}", Name = nameof(DeleteNote))]
        public IActionResult DeleteNote(int favID)
        {
            // note is bool to indicate whether the note was deleted or not
            var note = _dataService.DeleteNote(favID);

            if (note) return Ok();
            return NotFound();
        }
    }
}
