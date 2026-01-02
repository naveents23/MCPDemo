// client to call mcp server  
// dotnet add package ModelContextProtocol --prerelease
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using System.ComponentModel.DataAnnotations;

class Client {
   static async Task Main (string[] args) {
      await new Client ().StdClientDemo ();
   }

   #region Demo Methods ---------------------------------------------
   public async Task HttpClientServerDemo () {
      // ------------- MSFT document mcp
      var clientTransOption = new HttpClientTransportOptions {
         Endpoint = new Uri ("https://learn.microsoft.com/api/mcp"),
         Name = "client for MSFT document mcp",
      };

      var httpClientTransport = new HttpClientTransport (clientTransOption);

      var mcpClient = await McpClient.CreateAsync (httpClientTransport);
      await PrintListOfTools (mcpClient);
      Dictionary<string, object?> query = new () { ["query"] = "explain c#" };
      string toolName = "microsoft_docs_search";
      await CallTool (mcpClient, toolName, query);
   }

   // Example taken from
   // https://github.com/modelcontextprotocol/csharp-sdk
   public async Task StdClientDemo () {
      // 
      var clientTransport = new StdioClientTransport (new StdioClientTransportOptions {
         Name = "mcp_server",
         Command = "dotnet",
         Arguments = ["run", "--project", @"E:\MCP\MCP_Server\MCP_Server.csproj"], // Add your project path 
      });

      // this method is used to connect mcp server
      var mcpClient = await McpClient.CreateAsync (clientTransport);

      // Print the list of tools available from the server.
      await PrintListOfTools (mcpClient);

      // Invoke tool 
      await CallTool (mcpClient, toolName: "hello_world", argument: null);

      // Invoke tool with  argument
      Dictionary<string, object?> arg = new () { ["msg"] = "hi" }; // msg is parameter name
      await CallTool (mcpClient, toolName: "echo", argument: arg);

      // Invoke tool with two argument
      Dictionary<string, object> args = new () { ["a"] = 5, ["b"] = 5, };
      await CallTool (mcpClient, toolName:"add_two_number" ,argument: args);
   }
   #endregion


   #region Helper method --------------------------------------------
   async Task PrintListOfTools (McpClient? mcpClient) { // Example of client transport Http client transport,Stdio client transport.
      if (mcpClient == null) return;
      // Print the list of tools available from the server.
      foreach (var tool in await mcpClient!.ListToolsAsync ()) {
         Console.WriteLine ($"Name: {tool.Name}");
         Console.WriteLine ($"Description: {tool.Description}");
         Console.WriteLine ();
      }
   }

   async Task CallTool (McpClient mcpClient, string toolName, IReadOnlyDictionary<string, object?> argument) {
      var result = await mcpClient.CallToolAsync (toolName, argument);
      // echo always returns one and only one text content object
      Console.WriteLine (result.Content.OfType<TextContentBlock> ().First ().Text);
   }

   // Todo
   async Task ChatClient (McpClient client, string prompt) {
   }
   #endregion
}