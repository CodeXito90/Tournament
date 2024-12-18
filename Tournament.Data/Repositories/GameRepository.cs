﻿using Tournament.Core.Repositories;
using Tournament.Core;
using Tournament.Data.Data;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly Context _context;

        public GameRepository(Context context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Game>> GetAllByTournamentAsync(int tournamentId)
        {
            return await _context.Games
                .Where(g => g.TournamentId == tournamentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game?> GetAsync(int id, int tournamentId)
        {
            return await _context.Games
                .FirstOrDefaultAsync(g => g.Id == id && g.TournamentId == tournamentId);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Games.AnyAsync(g => g.Id == id);
        }

        public void Add(Game game)
        {
            _context.Games.Add(game);
        }

        public void Update(Game game)
        {
            _context.Games.Update(game);
        }

        public void Remove(Game game)
        {
            _context.Games.Remove(game);
        }

        //public Task GetByTitleAsync(string title)
        //{
        //    throw new NotImplementedException();
        //}

        Task<Game?> IGameRepository.GetByTitleAsync(string title)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetGameCountForTournamentAsync(int tournamentId)
        {
            return await _context.Games.CountAsync(g => g.TournamentId == tournamentId);
        }
    }
}
