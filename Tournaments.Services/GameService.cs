using AutoMapper;
using Services.Contract;
using Tournament.Core;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Middleware;
using Tournament.Core.Repositories;


namespace Tournament.Services
{
    public class GameService : IGameService
    {
        private readonly IUoW _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUoW unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedList<GameDto>> GetAllGamesAsync(int tournamentId, GameParameters parameters)
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            var gameDtos = _mapper.Map<IEnumerable<GameDto>>(games);

            return PagedList<GameDto>.ToPagedList(gameDtos, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<GameDto> GetGameAsync(int gameId, bool trackChanges = false)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(gameId);
            if (game == null)
                throw new GameNotFoundException(gameId);

            return _mapper.Map<GameDto>(game);
        }

        public async Task<GameDto> CreateGameAsync(int tournamentId, GameForCreationDto gameDto)
        {
            // Check if tournament exists
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId, includeGames: true);
            if (tournament == null)
                throw new TournamentNotFoundException(tournamentId);

            // Check game limit
            var gameCount = await _unitOfWork.GameRepository.GetGameCountForTournamentAsync(tournamentId);
            if (gameCount >= 10)
                throw new MaxGamesExceededException(tournamentId);

            var game = _mapper.Map<Game>(gameDto);
            game.TournamentId = tournamentId;

            _unitOfWork.GameRepository.Add(game);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<GameDto>(game);
        }

        public async Task<bool> UpdateGameAsync(int id, GameForUpdateDto gameDto)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
                return false; // Game does not exist

            // Update the game entity with the DTO properties
            _mapper.Map(gameDto, game);

            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.CompleteAsync();
            return true; // Successfully updated
        }


        public async Task DeleteGameAsync(int tournamentId, int gameId)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(gameId);
            if (game == null || game.TournamentId != tournamentId)
                throw new GameNotFoundException(gameId);

            _unitOfWork.GameRepository.Remove(game);
            await _unitOfWork.CompleteAsync();
        }
    }
}