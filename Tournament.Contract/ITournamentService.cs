using Tournament.Core.Dto;
using Tournament.Core;

namespace Services.Contract 
{
    public interface ITournamentService
    {
        Task<PagedList<TournamentDto>> GetAllTournamentsAsync(TournamentParameters parameters, bool trackChanges = false);
        Task<TournamentDto> GetTournamentAsync(int tournamentId, bool trackChanges = false);
        Task<TournamentDto> CreateTournamentAsync(TournamentForCreationDto tournament);
        Task UpdateTournamentAsync(int tournamentId, TournamentForUpdateDto tournamentForUpdate, bool trackChanges = false);
        Task DeleteTournamentAsync(int tournamentId, bool trackChanges = false);
    }
}
