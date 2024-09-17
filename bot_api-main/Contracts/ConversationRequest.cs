using System.ComponentModel.DataAnnotations;

namespace Bot.Api.Contracts
{
    public class ConversationRequest
    {
        [Required(ErrorMessage = "Device id is required.")]
        public string Deviceid { get; set; } = string.Empty;
        [Required(ErrorMessage = "Message is required.")]
        public string InputMessage { get; set; } = string.Empty;
        [Required(ErrorMessage = "Token is required.")]
        public string Token { get; set; } = string.Empty;
        [Required(ErrorMessage = "ConversationId is required.")]
        public string ConversationId { get; set; } = string.Empty;
    }
}
