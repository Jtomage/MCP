from fastmcp import FastMCP
from starlette.middleware import Middleware
from starlette.middleware.cors import CORSMiddleware
import mssql_python
import json
import logging
import sys
import os

# configure basic logging
logging.basicConfig(level=logging.INFO, format='%(name)s - %(levelname)s - %(message)s')
logger = logging.getLogger("sql-server-mcp")

# Initiate the server instance
mcp = FastMCP("sql-server-mcp")

# Add CORS Middleware
middleware = [
    Middleware(
        CORSMiddleware,
        allow_origins=["*"],  # Allow all origins; use specific origins for security
        allow_methods=["GET", "POST", "DELETE", "OPTIONS"],
        allow_headers=[
            "mcp-protocol-version",
            "mcp-session-id",
            "Authorization",
            "Content-Type",
        ],
        expose_headers=["mcp-session-id"],
    )
]

app = mcp.http_app(
    middleware=middleware, 
    transport="streamable-http",
    stateless_http=True    
)

##############################
# Sql Server Connection
##############################

def get_db_connection():
    conn_str = os.getenv("ConnectionString", 
        "SERVER=localhost,1433;DATABASE=Northwind;UID=sa;PWD=Password1;TrustServerCertificate=yes;"
    )
    return mssql_python.connect(conn_str)

def execute_to_json(sql, params=None) -> str:
    try:
        with get_db_connection() as conn:
            cursor = conn.cursor()

            if params is not None:
                if not isinstance(params, (list, tuple)):
                    params = (params,)
                cursor.execute(sql, params)
            else:
                cursor.execute(sql)

            columns = [column[0] for column in cursor.description]

            resultFetch = cursor.fetchall()

            results = []
            for row in resultFetch:
                results.append(dict(zip(columns, row)))

            return json.dumps(results, indent=2, default=str) # default=str handles dates/decimals
    except Exception as e:
        return f"Error executing query: {str(e)}"            

##############################
# Add functions here
##############################

# --- RESOURCE: The "Map" ---
@mcp.resource("db://tables")
def get_database_tables() -> str:
    sql = """
        SELECT TABLE_SCHEMA as 'Table Schema', TABLE_NAME as 'Table Name', TABLE_TYPE as 'Table Type' 
        FROM INFORMATION_SCHEMA.TABLES 
        WHERE TABLE_TYPE = 'BASE TABLE'
    """
    return execute_to_json(sql)

@mcp.resource("db://tables/{table_name}/schmea")
def get_table_columns(table_name: str) -> str:
    sql = """
        SELECT TABLE_SCHEMA as 'Table Schema', TABLE_NAME as 'Table Name',
        COLUMN_NAME as 'Column Name', IS_NULLABLE as 'Is Nullable', DATA_TYPE as 'Data Type'
        FROM INFORMATION_SCHEMA.COLUMNS 
        WHERE TABLE_NAME = ?
    """
    return execute_to_json(sql, table_name)


# --- TOOL: The "Executor" ---
@mcp.tool(description="Execute a SQL query and returns result in JSON")
def execute_sql(sql: str) -> str:
    return execute_to_json(sql)

# run mcp server
if __name__ == "__main__":
    logger.info("Starting MCP Server")
    import uvicorn    
    try:
        uvicorn.run(
            app, 
            host=os.getenv("MCP_HOST","127.0.0.1"),
            port=int(os.getenv("MCP_PORT","7071"))
        )

        # mcp.run(
        #     transport="streamable-http",
        #     stateless_http=True,
        #     host=os.getenv("MCP_HOST","127.0.0.1"),
        #     port=int(os.getenv("MCP_PORT","7071"))
        # )
    except Exception as e:
        logger.error(f"Server error: {str(e)}")
        sys.exit(1)