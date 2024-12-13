using AutoMapper;
using Tournament.Core.Dto;
using Tournament.Core.Repositories;
using Services.Contract;
using Tournament.Core.Entities;
using Tournament.Core;
using Tournament.Core.Middleware;

namespace Tournaments.Services
{  
        public class TournamentService : ITournamentService
        {
            private readonly IUoW _unitOfWork;
            private readonly IMapper _mapper;

            public TournamentService(IUoW unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<PagedList<TournamentDto>> GetAllTournamentsAsync(TournamentParameters parameters, bool trackChanges = false)
            {
                var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync(includeGames: true);
                var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);

                return PagedList<TournamentDto>.ToPagedList(tournamentDtos, parameters.PageNumber, parameters.PageSize);
            }

            public async Task<TournamentDto> GetTournamentAsync(int tournamentId, bool trackChanges = false)
            {
                var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId, includeGames: true);
                if (tournament == null)
                    throw new TournamentNotFoundException(tournamentId);

                return _mapper.Map<TournamentDto>(tournament);
            }

            public async Task<TournamentDto> CreateTournamentAsync(TournamentForCreationDto tournamentDto)
            {
                var tournament = _mapper.Map<TournamentDetails>(tournamentDto);
                _unitOfWork.TournamentRepository.Add(tournament);
                await _unitOfWork.CompleteAsync();

                return _mapper.Map<TournamentDto>(tournament);
            }

            public async Task UpdateTournamentAsync(int tournamentId, TournamentForUpdateDto tournamentForUpdate, bool trackChanges = false)
            {
                var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId, includeGames: false);
                if (tournament == null)
                    throw new TournamentNotFoundException(tournamentId);

                _mapper.Map(tournamentForUpdate, tournament);
                await _unitOfWork.CompleteAsync();
            }

            public async Task DeleteTournamentAsync(int tournamentId, bool trackChanges = false)
            {
                var tournament = await _unitOfWork.TournamentRepository.GetAsync(tournamentId, includeGames: false);
                if (tournament == null)
                    throw new TournamentNotFoundException(tournamentId);

                _unitOfWork.TournamentRepository.Remove(tournament);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
