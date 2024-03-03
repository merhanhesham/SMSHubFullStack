using System.ComponentModel.DataAnnotations;

namespace SMSHub.APIs.DTOs
{
    public class CreateSenderIdDto
    {
       // public int Id { get; set; }
        [Required]
        public string SenderIdText { get; set; }

        public string Description { get; set; } // Optional

        public bool IsActive { get; set; }
        public string UserId { get; set; }
    }
}
