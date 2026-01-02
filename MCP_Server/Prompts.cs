// Expose prompts tp client

using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCP_Server;

[McpServerPromptType]
public static class Prompts {
   [McpServerPrompt, Description ("Creates a prompt to summarize the provided message.")]
   public static ChatMessage Summarize ([Description ("The content to summarize")] string content) =>
       new (ChatRole.User, $"Please summarize this content into a single sentence: {content}");
}

