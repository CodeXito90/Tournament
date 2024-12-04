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

        // GET: api/Game
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGame()
        {
            var games = await _uow.GameRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<GameDto>>(games));
        }

        // GET: api/Game/5
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

        // PUT: api/Game/5
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
                return StatusCode(500);
            }

            return NoContent();
        }

        // POST: api/Game
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
                return StatusCode(500);
            }

            return CreatedAtAction("GetGame", new { id = game.Id }, _mapper.Map<GameDto>(game));
        }

        // DELETE: api/Game/5
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
                return StatusCode(500);
            }

            return NoContent();
        }

        // PATCH: api/Game/5
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
                return StatusCode(500);
            }

            return NoContent();
        }
    }
}

