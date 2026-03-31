using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using NorthwindMCP;
using NorthwindMCP.Northwind;

var builder = WebApplication.CreateBuilder(args);

// Add NorthwindContext
builder.AddDatabaseContext();

// bind northwind settings
var northwindConfig = new NorthwindMCPConfig();
builder.Services.Configure<NorthwindMCPConfig>(builder.Configuration);


// add services
builder.Services.AddNorthwindServices();

// Setup Mcp Server
builder.Services.AddMcpServer()
  .WithHttpTransport(options =>
  {
    // Sessions are only necessary when the server needs to send requests
    // to the client, push unsolicited notifications, or maintain
    // per-client state across requests.
    options.Stateless = true;
  })
  .WithToolsFromAssembly();

// cors
builder.Services.AddCors();

var app = builder.Build();

app.MapMcp("/mcp");

app.UseCors(policy => policy
  .AllowAnyOrigin()
  .AllowAnyMethod()
  .AllowAnyHeader());

var config = app.Services.GetRequiredService<IOptions<NorthwindMCPConfig>>().Value;

app.Run(config.NetworkBinding);