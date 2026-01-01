# setup Server
- create console application.
- install the nuget package 
```bash
dotnet add package ModelContextProtocol --prerelease
dotnet add package Microsoft.Extensions.Hosting
dotnet new gitignore
```
configure mcp.json file following example are globle mcp
```json
{
	"servers": {

		// Microsoft Learn MCP server configuration
		"microsoft-learn": {
			"type": "http",
			"url": "https://learn.microsoft.com/api/mcp"
		},

		// Sample MCP server configuration
		"testdemo": {
			"type": "stdio",
			"command": "dotnet",
			"args": [
				"run",
				"--project",
				"E:\\SampleMcpServer\\SampleMcpServer.csproj"
			]
		},

		// Another  MCP server configuration
		"mcp_server": {
			"type": "stdio",
			"command": "dotnet",
			"args": [
				"run",
				"--project",
				"C:\\MCP\\MCP\\MCP_Server.csproj"
			]
		}
      
	},
	"inputs": []
}
```

# using mcp template
```bash
dotnet new install Microsoft.McpServer.ProjectTemplates
dotnet new mcpserver -n SampleMcpServer
dotnet new gitignore
dotnet solution migrate SampleMcpServer.sln    // to conver into slnx file
dotnet build
```
Follow Quick guide link.

# Resources
[githubmcp](https://github.com/mcp)
[quick guide](https://learn.microsoft.com/en-us/dotnet/ai/quickstarts/build-mcp-server)
[Donet](https://www.youtube.com/watch?v=4zkIBMFdL2w)
[mcpSdk](https://github.com/modelcontextprotocol/csharp-sdk)



# Extra
[CentralizedPackageManagement](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)