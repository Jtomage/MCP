using ModelContextProtocol.Server;
using NorthwindMCP.Northwind;
using System.ComponentModel;

namespace NorthwindMCP.Tools
{
  [McpServerToolType]
  [Description("Tools for sql server, returns all data in JSON")]
  public class SqlServerTools
  {

    private readonly NorthwindRepository _repo;
    public SqlServerTools(NorthwindRepository repo)
    {
      _repo = repo;
    }

    [McpServerTool]
    [Description("List all available tables in the database")]
    public async Task<string> ListTables()
    {
      return await _repo.ListTables();
    }

    [McpServerTool]
    [Description("List all available colunms for a table")]
    public async Task<string> GetColumns(string tableName)
    {
      return await _repo.GetColumns(tableName);
    }

    [McpServerTool]
    [Description("List all available views in the database")]
    public async Task<string> ListViews()
    {
      return await _repo.ListViews();
    }

    [McpServerTool]
    [Description("Executes a SQL query and returns the results as JSON.")]
    public async Task<string> ExecuteSql(string query)
    {
      return await _repo.ExecuteQuery(query);
    }

  }
}
