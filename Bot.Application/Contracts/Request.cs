using Microsoft.Bot.Connector.DirectLine;

namespace Bot.Application.Contracts
{
    public class Request
    {
        public string Type { get; set; }
        public ChannelAccount From { get; set; }
        public string Text { get; set; }
        public string TextFormat { get; set; }
        public string Locale { get; set; }
    }
}
