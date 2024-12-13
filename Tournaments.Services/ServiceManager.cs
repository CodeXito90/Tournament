using AutoMapper;
using Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;

namespace Tournaments.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<ITournamentService> _tournamentService;
        private readonly Lazy<IGameService> _gameService;

        public ServiceManager(
            ITournamentService tournamentService,
            IGameService gameService)
        {
            _tournamentService = new Lazy<ITournamentService>(() => tournamentService);
            _gameService = new Lazy<IGameService>(() => gameService);
        }

        public ITournamentService TournamentService => _tournamentService.Value;
        public IGameService GameService => _gameService.Value;
    }
}
