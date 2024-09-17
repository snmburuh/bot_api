using Microsoft.Bot.Connector.DirectLine;
using System.Text;
using Bot.Application.Abstractions.Messaging;
using Bot.Application.Contracts;
using Bot.Application.HttpHandler;

namespace Bot.Application.Bot.Commands
{
    internal class CreateConversationHandler : ICommandHandler<CreateConversationCommand, string>
    {
        private readonly IHttpClientFactoryService _httpClientFactoryService;
        private readonly AppSettings _appSettings;
        public CreateConversationHandler(IHttpClientFactoryService httpClientFactoryService,
            AppSettings appSettings)
        {
            _httpClientFactoryService = httpClientFactoryService;
            _appSettings = appSettings;
        }
        public async Task<string> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var results = string.Empty;
            var tokenResponse = new DirectLineToken();
            string conversationtId = string.Empty;

            if (!string.IsNullOrEmpty(request.deviceid) && request.inputMessage.ToLower() != _appSettings.EndConversationMessage.ToLower())
            {
                string inputMessage;

                    Request _request = new Request
                    {
                        Type = ActivityTypes.Message,
                        From = new ChannelAccount { Id = "userId", Name = "userName" },
                        Text = request.inputMessage,
                        TextFormat = "plain",
                        Locale = "en-Us",
                    };

                    var reponse = await _httpClientFactoryService.Conversation(request.conversationId,request.token, _request);
                  //  Thread.Sleep(3000);
                    // Get bot response using directlinClient
                    List <Activity> responses = await _httpClientFactoryService.GetBotResponseActivitiesAsync(request.conversationId,request.token);
                results =  BotReply2(responses);
                

            }
            return results;
        }



        /// <summary>
        /// Print bot reply to console
        /// </summary>
        /// <param name="responses">List of DirectLine activities <see cref="https://github.com/Microsoft/botframework-sdk/blob/master/specs/botframework-activity/botframework-activity.md"/>
        /// </param>     
        private string BotReply2(List<Activity> responses)
        {
            StringBuilder output = new StringBuilder();

            responses?.ForEach(responseActivity =>
            {
                if (!string.IsNullOrEmpty(responseActivity.Text))
                {
                    output.AppendLine(string.Join(Environment.NewLine, responseActivity.Text));
                }

                if (responseActivity.SuggestedActions != null && responseActivity.SuggestedActions.Actions != null)
                {
                    var options = responseActivity.SuggestedActions.Actions.Select(a => a.Title).ToList();
                    output.AppendLine($"\t{string.Join(" | ", options)}");
                }
            });

            return output.ToString();
        }

    }
}
