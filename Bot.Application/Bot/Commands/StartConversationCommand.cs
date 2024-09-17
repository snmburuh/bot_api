using Bot.Application.Abstractions.Messaging;

namespace Bot.Application.Bot.Commands
{
    public sealed record StartConversationCommand(string deviceId) : ICommand<StartConversationResponse>;

}
