using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace NorthwindMCP.Northwind;

public class NorthwindRepository
{

  private readonly NorthwindContext _context;

  public NorthwindRepository(NorthwindContext context)
  {
    _context = context;
  }

  public Task<string> ListTables()
  {
    var sql = "SELECT TABLE_SCHEMA as 'Table Schema', TABLE_NAME as 'Table Name', TABLE_TYPE as 'Table Type' " + "\n";
    sql += "FROM INFORMATION_SCHEMA.TABLES " + "\n";
    sql += "WHERE TABLE_TYPE = 'BASE TABLE'";
    return ExecuteQuery(sql);
  }

  public Task<string> ListViews()
  {
    var sql = "SELECT TABLE_SCHEMA as 'Table Schema', TABLE_NAME as 'Table Name', TABLE_TYPE as 'Table Type' " + "\n";
    sql += "FROM INFORMATION_SCHEMA.TABLES " + "\n";
    sql += "WHERE TABLE_TYPE = 'VIEW'";
    return ExecuteQuery(sql);
  }

  public Task<string> GetColumns(string tableName)
  {
    var sql = "SELECT TABLE_SCHEMA as 'Table Schema', TABLE_NAME as 'Table Name', " + "\n";
    sql += "COLUMN_NAME as 'Column Name', IS_NULLABLE as 'Is Nullable', DATA_TYPE as 'Data Type' " + "\n";
    sql += "FROM INFORMATION_SCHEMA.COLUMNS " + "\n";
    sql += $"WHERE TABLE_NAME = '{tableName}'";

    return ExecuteQuery(sql);
  }

  public async Task<string> ExecuteQuery(string query)
  {
    var results = new List<Dictionary<string, object>>();

    // going ado.net
    using (var command = _context.Database.GetDbConnection().CreateCommand())
    {
      command.CommandText = query;

      // check if connection is open
      if (command.Connection?.State != System.Data.ConnectionState.Open)
        await _context.Database.OpenConnectionAsync();

      using (var reader = await command.ExecuteReaderAsync())
      {
        while (await reader.ReadAsync())
        {
          var row = new Dictionary<string, object>();
          for (int i = 0; i < reader.FieldCount; i++)
            row[reader.GetName(i)] = reader.GetValue(i);

          results.Add(row);
        }
      }
    }

    // serialize results
    var json = JsonSerializer.Serialize(results);

    return json;
  }
}