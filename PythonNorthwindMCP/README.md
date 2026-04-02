# Python Northwind MCP

Learning how to build a HTTP based MCP server in Python. Also learning Python as I do this.

### Start MCP Server

1. built using uv
2. from the PythonMCP folder
3  run `uv run server.py`

### Start Docker Compose stack

1. Docker compose up -d

### Connection AI to MCP Server

**Connection steps same as .Net version**

1. Open settings
2. Select the tools tab
3. Check
    1. Enable Toolcalling
    2. Automatically Execute Tools
    3. Use CORS Proxy for MCP
    4. Include KoboldCpp MCP Bridge
4. MCP Server URL to the Northwind DB Container
    1. http://host.docker.internal:7071/mcp
5. On Connection a listing of tools will appear

### Testing MCP Server without AI

use MCP Inspector again like the .Net version

`npx @modelcontextprotocol/inspector`

It will open in the browser

Configuration

* Transport Type = Steamable HTTP
* URL = http://localhost:7071/mcp
* Connection Type = Proxy