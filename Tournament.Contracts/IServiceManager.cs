using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Tournament.Contracts
{
    public interface IServiceManager
    {
        ITournamentService TournamentService { get; }
        IGameService GameService { get; }
    }

    public interface ITournamentService
    {
        Task<(IEnumerable<TournamentDto> tournaments, Metadata metaData)> GetAllTournamentsAsync(int pageNumber, int pageSize);
        Task<TournamentDto> GetTournamentByIdAsync(int id);
        Task<TournamentDto> CreateTournamentAsync(TournamentForCreationDto tournamentForCreation);
        // Add other methods as needed
    }

    public interface IGameService
    {
        Task<(IEnumerable<GameDto> games, MetaData metaData)> GetAllGamesAsync(int pageNumber, int pageSize);
        Task<GameDto> GetGameByIdAsync(int id);
        Task<GameDto> CreateGameAsync(GameForCreationDto gameForCreation);
        // Add other methods as needed
    }
}

