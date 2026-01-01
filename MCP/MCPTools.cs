using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public sealed class EchoTools {
   [McpServerTool,Description("Echoes the message back to the client.")]
   public static string Echo (string msg) => $"Echo from mcp server and you typed {msg}";

   [McpServerTool,Description("Reverse the given string")]
   public static string Reverse(string str )=>str.Reverse().ToString()!;
}