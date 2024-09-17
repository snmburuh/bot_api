using Microsoft.Bot.Connector.DirectLine;
using Bot.Application.Abstractions.Messaging;

namespace Bot.Application.Bot.Commands
{
    public sealed record CreateConversationCommand(string deviceid,string inputMessage, string token, string conversationId) : ICommand<string>;

}
