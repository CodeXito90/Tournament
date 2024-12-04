using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public TournamentController(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Tournament
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails()
        {
            var tournaments = await _uow.TournamentRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<TournamentDto>>(tournaments));
        }

        // GET: api/Tournament/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TournamentDto>(tournament));
        }

        // PUT: api/Tournament/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDetails tournamentDetails)
        {
            if (id != tournamentDetails.Id)
            {
                return BadRequest();
            }

            if (!await _uow.TournamentRepository.AnyAsync(id))
            {
                return NotFound();
            }

            _uow.TournamentRepository.Update(tournamentDetails);

            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        // POST: api/Tournament
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentDetails tournamentDetails)
        {
            _uow.TournamentRepository.Add(tournamentDetails);

            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500);
            }

            return CreatedAtAction("GetTournamentDetails", new { id = tournamentDetails.Id }, _mapper.Map<TournamentDto>(tournamentDetails));
        }

        // DELETE: api/Tournament/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            _uow.TournamentRepository.Remove(tournament);

            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        // PATCH: api/Tournament/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTournamentDetails(int id, [FromBody] JsonPatchDocument<TournamentDetails> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var tournament = await _uow.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(tournament, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500);
            }

            return NoContent();
        }
    }
}

