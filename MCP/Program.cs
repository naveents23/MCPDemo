// MCP server
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using System.Text.Json;

class Program {
   // Create a host add mcp server with standard input and output with tools from assembly (From mcp tool attribute).
   static async Task Main (string[] args) {
      var builder = Host.CreateApplicationBuilder (settings: null);
      builder.Services.AddMcpServer ()
         .WithStdioServerTransport ()
         .WithTools<EchoTools> ()
         .WithToolsFromAssembly ();
         

      await builder.Build ().RunAsync ();

      //  Configuring mcp server
      McpServerOptions options = new () {
         ServerInfo = new Implementation { Name = "MyServer", Version = "1.0.0" },
         Handlers = new McpServerHandlers () {
            ListToolsHandler = (request, cancellationToken) =>
                ValueTask.FromResult (new ListToolsResult {
                   Tools =
                    [
                        new Tool
                    {
                        Name = "echo",
                        Description = "Echoes the input back to the client.",
                        //InputSchema = JsonSerializer.Deserialize<JsonElement>("""
                        //    {
                        //        "type": "object",
                        //        "properties": {
                        //          "message": {
                        //            "type": "string",
                        //            "description": "The input to echo back"
                        //          }
                        //        },
                        //        "required": ["message"]
                        //    }
                        //    """),
                      
                    }
                    ]
                }),

            CallToolHandler = (request, cancellationToken) => {
               if (request.Params?.Name == "echo") {
                  if (request.Params.Arguments?.TryGetValue ("message", out var message) is not true) {
                     throw new McpProtocolException ("Missing required argument 'message'", McpErrorCode.InvalidParams);
                  }

                  return ValueTask.FromResult (new CallToolResult {
                     Content = [new TextContentBlock { Text = $"Echo: {message}" }]
                  });
               }

               throw new McpProtocolException ($"Unknown tool: '{request.Params?.Name}'", McpErrorCode.InvalidRequest);
            }
         }
      };

      await using McpServer server = McpServer.Create (new StdioServerTransport ("MyServer"), options);
      await server.RunAsync ();

   }
}