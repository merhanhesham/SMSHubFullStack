namespace SMSHub.APIs.DTOs
{
    public class AssignSenderIdsDto
    {
        public string UserId { get; set; }
        public List<string> SenderIdTexts { get; set; }
    }
}
