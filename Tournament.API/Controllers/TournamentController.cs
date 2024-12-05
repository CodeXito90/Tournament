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

        // Dependency injection of IUoW and IMapper
        // This allows for loose coupling and easier unit testing
        public TournamentController(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Tournament
        // Retrieves all tournaments, optionally including related games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails(bool includeGames = false)
        {
            var tournaments = await _uow.TournamentRepository.GetAllAsync(includeGames);

            if (tournaments == null || !tournaments.Any())
            {
                return NotFound("No tournaments found.");
            }

            var result = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(result);
        }


        // GET: api/Tournament/5
        // Retrieves a specific tournament by id, optionally including related games
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id, [FromQuery] bool includeGames = false)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id, includeGames);

            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TournamentDto>(tournament));
        }

        // PUT: api/Tournament/5
        // Updates a specific tournament
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
                // The Unit of Work pattern is used here to save changes
                // This ensures that all operations within the transaction are successful, or none are
                await _uow.CompleteAsync();
            }
            catch
            {
                // If an error occurs during save, return a 500 Internal Server Error
                return StatusCode(500);
            }

            return NoContent();
        }

        // POST: api/Tournament
        // Creates a new tournament
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
        // Deletes a specific tournament
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id, true);
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
        // Partially updates a tournament
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTournamentDetails(int id, [FromBody] JsonPatchDocument<TournamentDetails> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var tournament = await _uow.TournamentRepository.GetAsync(id, true);

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

