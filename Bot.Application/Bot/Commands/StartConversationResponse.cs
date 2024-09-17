namespace Bot.Application.Bot.Commands
{
    public class StartConversationResponse
    {
        public string conversationId { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public int expires_in { get; set; }
        public string streamUrl { get; set; } = string.Empty;
        public string referenceGrammarId { get; set; } = string.Empty;
    }
}
