namespace Bot.Application.Contracts
{
    public class DirectLineToken
    {
        public string Token { get; set; }
        public int Expires_in { get; set; }
        public string ConversationId { get; set; }
    }
}
