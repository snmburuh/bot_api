using Microsoft.Bot.Connector.DirectLine;
using Bot.Application.Bot.Commands;
using Bot.Application.Contracts;

namespace Bot.Application.HttpHandler
{
    public interface IHttpClientFactoryService
    {
        public Task<DirectLineToken> GetTokenAsync();
        Task<RegionalChannelSettingsDirectLine> GetRegionalChannelSettingsDirectline(string tokenEndpoint);
        Task<StartConversationResponse> StartConversation(string token);
        Task<List<Activity>> GetBotResponseActivitiesAsync(string conversationtId, string token);
        Task<DirectLineToken> GetTokenFromCacheAsync(string deviceId,
            CancellationToken cancellationToken);
        Task<dynamic>  Conversation(string conversationtId, string token, Request request);
    }
}
