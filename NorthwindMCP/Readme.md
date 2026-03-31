# Northwind MCP

Learning how to build an HTTP based MCP server with .NET as I see pods of MCP servers to connect to reference databases. 

Using the Northwind Data from Microsoft as a sample data to build a generic SQL server MCP Server that would be able to query the database

### Start

use docker-compose up -d to start the docker file

**Notes**

* had to add cors policy to allow all to get it run
* do not have a gpu on this dev computer so the gpu and threads will probably need to change with the computer
* TODO: Create MCPConfig.json

### Testing MCP Server without AI

use MCP Inspector

`npx @modelcontextprotocol/inspector`

It will open in the browser

Configuration

* Transport Type = Steamable HTTP
* URL = http://localhost:7071/
* Connection Type = Proxy

This should connect to the MCP server and be able to list all the tools

### Connection AI to MCP Server

**Connection steps**

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

### Screenshot working

![Kobold_views](screenshot\Kobold_views.png)

### references

* https://csharp.sdk.modelcontextprotocol.io/concepts/getting-started.html