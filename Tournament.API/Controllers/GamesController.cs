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
    public class GamesController : ControllerBase
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public GamesController(IUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames([FromQuery] string title = null)
        {
            IEnumerable<Game> games;
            if (string.IsNullOrEmpty(title))
            {
                games = await _uow.GameRepository.GetAllAsync();
            }
            else
            {
                games = (IEnumerable<Game>)await _uow.GameRepository.GetByTitleAsync(title);
            }
            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GameDto>(game));
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            if (!await _uow.GameRepository.AnyAsync(id))
            {
                return NotFound();
            }

            _uow.GameRepository.Update(game);

            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the game.");
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(Game game)
        {
            _uow.GameRepository.Add(game);

            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "An error occurred while creating the game.");
            }

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, _mapper.Map<GameDto>(game));
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _uow.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _uow.GameRepository.Remove(game);

            try
            {
                await _uow.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the game.");
            }

            return NoContent();
        }

        // PATCH: api/Games/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchGame(int id, [FromBody] JsonPatchDocument<Game> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var game = await _uow.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(game, ModelState);

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
                return StatusCode(500, "An error occurred while updating the game.");
            }

            return NoContent();
        }
    }
}

