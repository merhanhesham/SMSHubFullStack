using SMSHub.Core.Entities;

namespace SMSHub.APIs.DTOs
{
    public class SmsRequestDto
    {
        // Support multiple recipient phone numbers
        public ICollection<string> Recipients { get; set; } = new List<string>();
        public string Message { get; set; }

        // Support multiple Sender IDs
        public string From { get; set; }


    }
}
