using Bot.Application.Abstractions.Messaging;
using Bot.Application.Contracts;
using Bot.Application.HttpHandler;

namespace Bot.Application.Bot.Commands
{
    internal class StartConversationHandler : ICommandHandler<StartConversationCommand, StartConversationResponse>
    {
        private readonly IHttpClientFactoryService _httpClientFactoryService;
        
        private readonly AppSettings _appSettings;
        public StartConversationHandler(IHttpClientFactoryService httpClientFactoryService,
            AppSettings appSettings)
        {
            _httpClientFactoryService = httpClientFactoryService;
            _appSettings = appSettings;
        }
        public async Task<StartConversationResponse> Handle(StartConversationCommand request,
            CancellationToken cancellationToken)
        {
            var tokenResponse = await _httpClientFactoryService.GetTokenFromCacheAsync(request.deviceId,cancellationToken);
            var data = await _httpClientFactoryService.StartConversation(tokenResponse.Token);
            return data;
        }
    }
}
