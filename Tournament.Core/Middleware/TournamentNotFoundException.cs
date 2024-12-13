namespace Tournament.Core.Middleware
{

    public class TournamentNotFoundException : Exception
    {
        public TournamentNotFoundException(int tournamentId)
            : base($"Tournament with id: {tournamentId} was not found.")
        {
        }
    }

    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(int gameId)
            : base($"Game with id: {gameId} was not found.")
        {
        }
    }

    public class MaxGamesExceededException : Exception
    {
        public MaxGamesExceededException(int tournamentId)
            : base($"Cannot add more than 10 games to tournament {tournamentId}.")
        {
        }
    }
}
