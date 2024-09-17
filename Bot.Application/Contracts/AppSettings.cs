namespace Bot.Application.Contracts
{
    public class AppSettings
    {
        public string BotId { get; set; }
        public string BotTenantId { get; set; }
        public string BotName { get; set; }
        public string BotTokenEndpoint { get; set; }
        public string BotconversationsEndpoint { get; set; }
        public string EndConversationMessage { get; set; }
        public string BotconversationActivitiesEndpoint { get; set; }
    }
}
