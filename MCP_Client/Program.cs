// client to call mcp 
// dotnet add package ModelContextProtocol --prerelease
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using System.Threading.Tasks;

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
      Dictionary<string, object> que = new () { ["query"] = "explain c#" };
      await ExecuteTool (mcpClient, "microsoft_docs_search", que);
   }

   // Example taken from
   // https://github.com/modelcontextprotocol/csharp-sdk
   public async Task StdClientDemo () {
      // 
      var clientTransport = new StdioClientTransport (new StdioClientTransportOptions {
         Name = "mcp_server",
         Command = "dotnet",
         Arguments = ["run", "--project", "C:\\MCP\\MCP\\MCP_Server.csproj"],
      });

      // this method is used to connect mcp server
      var client = await McpClient.CreateAsync (clientTransport);

      // Print the list of tools available from the server.
      foreach (var tool in await client.ListToolsAsync ()) {
         Console.WriteLine ($"{tool.Name} ({tool.Description})");
      }

      // Execute a tool (this would normally be driven by LLM tool invocations).
      var result = await client.CallToolAsync (
          "echo",
          new Dictionary<string, object?> () { ["message"] = "Hello MCP!" },
          cancellationToken: CancellationToken.None);

      // echo always returns one and only one text content object
      Console.WriteLine (result.Content.OfType<TextContentBlock> ().First ().Text);
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

   async Task ExecuteTool (McpClient mcpClient, string toolName, IReadOnlyDictionary<string, object?> query) {
      var result = await mcpClient.CallToolAsync (toolName, query);
      // echo always returns one and only one text content object
      Console.WriteLine (result.Content.OfType<TextContentBlock> ().First ().Text);
   }

   async Task ChatClient (McpClient client ,string prompt) {
      // Get available functions.
      var tools = await client.ListToolsAsync ();

      //// Call the chat client using the tools.
      //IChatClient chatClient ;
      //var response = await chatClient.GetResponseAsync (prompt, new () { Tools = [..tools] });

   }
   #endregion
}