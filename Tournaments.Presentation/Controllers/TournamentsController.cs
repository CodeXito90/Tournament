using Microsoft.AspNetCore.Mvc;
using Tournament.Core.Dto;
using Services.Contract;
using Tournament.Core;
using System.Text.Json;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TournamentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTournaments([FromQuery] TournamentParameters parameters)
        {
            var tournaments = await _serviceManager.TournamentService.GetAllTournamentsAsync(parameters);

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(new
                {
                    tournaments.CurrentPage,
                    tournaments.TotalPages,
                    tournaments.PageSize,
                    tournaments.TotalCount
                });

            return Ok(tournaments);
        }

        [HttpGet("{id}", Name = "GetTournament")]
        public async Task<IActionResult> GetTournament(int id)
        {
            var tournament = await _serviceManager.TournamentService.GetTournamentAsync(id);
            return Ok(tournament);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTournament([FromBody] TournamentForCreationDto tournament)
        {
            var createdTournament = await _serviceManager.TournamentService.CreateTournamentAsync(tournament);
            return CreatedAtRoute("GetTournament", new { id = createdTournament.Id }, createdTournament);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTournament(int id, [FromBody] TournamentForUpdateDto tournament)
        {
            await _serviceManager.TournamentService.UpdateTournamentAsync(id, tournament);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            await _serviceManager.TournamentService.DeleteTournamentAsync(id);
            return NoContent();
        }
    }
}