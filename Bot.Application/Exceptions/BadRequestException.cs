namespace Bot.Application.Exceptions
{
    public abstract class BadRequestException : System.Exception
    {
        protected BadRequestException(string message)
            : base(message)
        {
        }
    }
}
