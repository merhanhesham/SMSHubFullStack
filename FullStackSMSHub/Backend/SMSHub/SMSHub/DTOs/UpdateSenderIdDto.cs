namespace SMSHub.APIs.DTOs
{
    public class UpdateSenderIdDto 
    {
        public int Id { get; set; }
        public string SenderIdText { get; set; }

        public string Description { get; set; } // Optional

        public bool IsActive { get; set; }
        public string UserId { get; set; }
    }
}
