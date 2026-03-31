using Microsoft.EntityFrameworkCore;

namespace NorthwindMCP.Northwind
{
  public class NorthwindContext : DbContext
  {
    public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options) { }

  }
}
