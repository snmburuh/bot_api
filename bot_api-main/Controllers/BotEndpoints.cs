using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bot.Application.Bot.Commands;
using Bot.Api.Contracts;

namespace Bot.Api.Controllers
{
    public class BotEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            //ToDo: App token protection- reuse app engine logic to validate token passed by channel
            var group = app.MapGroup("api/Bot");

            group.MapPost("", StartConversation);

            group.MapPost("/conversation", ProcessConversation);

        }

        public static async Task<IResult> StartConversation([FromHeader] string deviceId,ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new StartConversationCommand(deviceId);
            var response = await sender.Send(command, cancellationToken);
            return Results.Ok(response);
        }

       public static async Task<IResult> ProcessConversation(ConversationRequest conversationRequest,
           ISender sender, CancellationToken cancellationToken)
        {
            var command = conversationRequest.Adapt<CreateConversationCommand>();
            var response = await sender.Send(command, cancellationToken);
            return Results.Ok(response);
        }
    }
}
