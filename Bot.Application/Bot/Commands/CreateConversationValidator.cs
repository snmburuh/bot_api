using FluentValidation;

namespace Bot.Application.Bot.Commands
{
    public sealed class CreateConversationValidator : AbstractValidator<CreateConversationCommand>
    {
        public CreateConversationValidator()
        {
            //RuleFor(x => x.deviceid).NotEmpty();

            //RuleFor(x => x.inputMessage).NotEmpty();
        }
    }
}
