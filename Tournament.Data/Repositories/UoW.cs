using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    //Keynotes: 
    // The Unit of Work class encapsulates the idea of a transaction
    // It coordinates the work of multiple repositories
    public class UoW : IUoW
    {
        private readonly Context _context;
        private ITournamentRepository _tournamentRepository;
        private IGameRepository _gameRepository;

        public UoW(Context context)
        {
            _context = context;
        }

        // Lazy loading of repositories
        // This ensures that repositories are only created when they're needed
        public ITournamentRepository TournamentRepository =>
            _tournamentRepository ??= new TournamentRepository(_context);

        public IGameRepository GameRepository =>
            _gameRepository ??= new GameRepository(_context);

        // CompleteAsync saves all changes made in this context to the database
        // This is where the actual database transaction occurs
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
