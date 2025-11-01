using InnHotel.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace InnHotel.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
  /// <summary>
  /// Overriding CreateHost to avoid creating a separate ServiceProvider per this thread:
  /// https://github.com/dotnet-architecture/eShopOnWeb/issues/465
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  protected override IHost CreateHost(IHostBuilder builder)
  {
    builder.UseEnvironment("Development"); // will not send real emails
    var host = builder.Build();
    host.Start();

    // Get service provider.
    var serviceProvider = host.Services;

    // Create a scope to obtain a reference to the database
    // context (AppDbContext).
    using (var scope = serviceProvider.CreateScope())
    {
      var scopedServices = scope.ServiceProvider;
      var db = scopedServices.GetRequiredService<AppDbContext>();

      var logger = scopedServices
          .GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

      // Reset Sqlite database for each test run
      // If using a real database, you'll likely want to remove this step.
      db.Database.EnsureDeleted();

      // Ensure the database is created.
      db.Database.EnsureCreated();

      try
      {
        // Can also skip creating the items
        //if (!db.ToDoItems.Any())
        //{
        // Seed the database with test data.
        SeedData.SeedApplicationDataAsync(db).Wait();
        //}
      }
      catch (Exception ex)
      {
        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {exceptionMessage}", ex.Message);
      }
    }

    return host;
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder
        .UseEnvironment("Test")
        .ConfigureAppConfiguration((context, config) =>
        {
          // Override connection string with test database connection
          var testConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgreSQLConnection") 
                                    ?? "Host=localhost;Port=5432;Database=innhotel_test;Username=postgres;Password=postgres";
          
          config.AddInMemoryCollection(new[]
          {
            new KeyValuePair<string, string?>("ConnectionStrings:PostgreSQLConnection", testConnectionString)
          });
        })
        .ConfigureServices(services =>
        {
          // Configure test dependencies here
          // Using PostgreSQL for functional tests to match production environment
        });
  }
}
