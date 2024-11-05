using System.Text.Json.Serialization;

namespace Chatbot.Api.Models
{
    public class ChatbotAgentMessage
    {
        public required string Role { get; set; }
        public required string Message { get; set; }
    }

    public class ChatbotAgentRequest
    {
         [JsonPropertyName("request")]
        public required List<ChatbotAgentMessage> Request { get; set; }
    }
}