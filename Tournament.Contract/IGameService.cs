using Tournament.Core.Dto;
using Tournament.Core;

namespace Services.Contract 
{
    public interface IGameService
    {

        Task<PagedList<GameDto>> GetAllGamesAsync(int tournamentId, GameParameters parameters);
        Task<GameDto> GetGameAsync(int gameId, int tournamentId, bool trackChanges = false);
        Task<GameDto> CreateGameAsync(int tournamentId, GameForCreationDto game);
        Task<bool>UpdateGameAsync(int gameId, int tournamentId, GameForUpdateDto gameForUpdate);
        Task DeleteGameAsync(int gameId, int tournamentId);
    }
       

}
