using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public static class EchoTools {
   [McpServerTool,Description("Echoes the message back to the client.")]
   public static string Echo (string msg) => $"Echo from mcp server tool and you typed {msg}";

   [McpServerTool,Description("Reverse the given string")]
   public static string Reverse(string str )=>str.Reverse().ToString()!;

   [McpServerTool, Description ("HelloWorld")]
   public static string HelloWorld () => "Hello World from mcp server tool!";

   [McpServerTool, Description ("Add two numbers")]
   public static string AddTwoNumber (int a ,int b) => (a + b).ToString();
}