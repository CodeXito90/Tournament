using Microsoft.AspNetCore.Mvc;
using Services.Contract;
using System.Text.Json;
using Tournament.Core;
using Tournament.Core.Dto;

namespace Tournaments.Presentation.Controllers
{
    [Route("api/tournaments/{tournamentId}/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GamesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/tournaments/{tournamentId}/games
        [HttpGet]
        public async Task<IActionResult> GetGames(int tournamentId, [FromQuery] GameParameters parameters)
        {
            var games = await _serviceManager.GameService.GetAllGamesAsync(tournamentId, parameters);

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(new
            {
                games.CurrentPage,
                games.TotalPages,
                games.PageSize,
                games.TotalCount
            });

            return Ok(games);
        }

        // GET: api/tournaments/{tournamentId}/games/{id}
        [HttpGet("{id}", Name = "GetGame")]
        public async Task<IActionResult> GetGame(int tournamentId, int gameId)
        {
            var game = await _serviceManager.GameService.GetGameAsync(tournamentId);
            if (game == null)
                return NotFound();

            return Ok(game);
        }

        // POST: api/tournaments/{tournamentId}/games
        [HttpPost]
        public async Task<IActionResult> CreateGame(int tournamentId, [FromBody] GameForCreationDto game)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdGame = await _serviceManager.GameService.CreateGameAsync(tournamentId, game);

            return CreatedAtRoute("GetGame", new { tournamentId, id = createdGame.Id }, createdGame);
        }

        // PUT: api/tournaments/{tournamentId}/games/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] GameForUpdateDto game)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Call the service method and store the result
            var result = await _serviceManager.GameService.UpdateGameAsync(id, game);

            // If the result is false, return a NotFound response
            if (!result)
                return NotFound();

            // Otherwise, return NoContent
            return NoContent();
        }


        // DELETE: api/tournaments/{tournamentId}/games/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int tournamentId, int gameId)
        {
            var game = await _serviceManager.GameService.GetGameAsync(tournamentId);
            if (game == null)
                return NotFound();

            await _serviceManager.GameService.DeleteGameAsync(tournamentId, gameId);
            return NoContent();
        }

    }
}
