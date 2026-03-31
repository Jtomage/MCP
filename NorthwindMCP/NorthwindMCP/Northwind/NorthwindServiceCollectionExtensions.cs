using Microsoft.EntityFrameworkCore;

namespace NorthwindMCP.Northwind;

public static class NorthwindServiceCollectionExtensions
{
  public static void AddDatabaseContext(this IHostApplicationBuilder builder)
  {
    // get the connection string
    var connectionStr = builder.Configuration.GetConnectionString("Northwind");

    Console.WriteLine("connectiong string: " + connectionStr);

    if (connectionStr == null)
      throw new NullReferenceException("Connection String is NULL");

    // set dbcontext
    builder.Services.AddDbContext<NorthwindContext>(options =>
    {
      options.UseSqlServer(connectionStr);
    });
  }

  public static void AddNorthwindServices(this IServiceCollection services)
  {
    services.AddScoped<NorthwindRepository>();
  }
}
