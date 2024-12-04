using Tournament.Data.Data;

namespace Tournament.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task SeedDataAsync(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<Context>();

                    // Ensure the database is created
                    await context.Database.EnsureCreatedAsync();

                    // Check if the database is already seeded
                    if (!context.TournamentDetails.Any())
                    {
                        // Call the seed method from SeedData class
                        await SeedData.InitializeAsync(context);
                    }
                }
                catch (Exception ex)
                {
                    // Log the error
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the database.");
                }
            }
        }
    }
}
