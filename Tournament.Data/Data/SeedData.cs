using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(Context context)
        {
            // Create some tournaments
            var tournaments = new List<TournamentDetails>
            {
                new TournamentDetails
                {
                    Title = "Summer Chess Championship",
                    StartDate = DateTime.Now.AddMonths(1),
                    Games = new List<Game>
                    {
                        new Game { Title = "Quarterfinal Match 1", Time = DateTime.Now.AddMonths(1).AddDays(7)},
                        new Game { Title = "Quarterfinal Match 2", Time = DateTime.Now.AddMonths(1).AddDays(14)}
                    }
                },
                new TournamentDetails
                {
                    Title = "Winter Poker Tournament",
                    StartDate = DateTime.Now.AddMonths(2),
                    Games = new List<Game>
                    {
                        new Game { Title = "Semi-Final Round", Time = DateTime.Now.AddMonths(2).AddDays(10)},
                        new Game { Title = "Final Round", Time = DateTime.Now.AddMonths(2).AddDays(20)}
                    }
                },
                new TournamentDetails
                {
                    Title = "Call Of Duty Winter Special",
                    StartDate = DateTime.Now.AddMonths(1),
                    Games = new List<Game>
                    {
                        new Game { Title = "Preliminaries", Time = DateTime.Now.AddMonths(2).AddDays(10)},
                        new Game { Title = "Semi-Finals", Time = DateTime.Now.AddMonths(2).AddDays(20)}
                    }
                },
                 new TournamentDetails
                {
                    Title = "Apex Legends Platinum Cup 2025",
                    StartDate = DateTime.Now.AddMonths(3),
                    Games = new List<Game>
                    {
                        new Game { Title = "Opening Game", Time = DateTime.Now.AddMonths(1).AddDays(10)},
                        new Game { Title = "Semi Final 1", Time = DateTime.Now.AddMonths(2).AddDays(10)},
                        new Game { Title = "Semi Final 2", Time = DateTime.Now.AddMonths(3).AddDays(10)},
                        new Game { Title = "Final", Time = DateTime.Now.AddMonths(4).AddDays(10)}
                    }
                }
            };
        
            // Add the tournaments to the context
            context.TournamentDetails.AddRange(tournaments);

            // Save changes to the database
            await context.SaveChangesAsync();
        }
    }
}
