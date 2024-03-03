using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSHub.Core.Entities
{
    public class SmsMessage : BaseEntity
    {
        //public List<string> To { get; set; }
        //public string From { get; set; } // Sender's phone number
        public string Message { get; set; } // Message content
        public DateTime SentTime { get; set; } // Time the message was sent

        public int SenderIdId { get; set; } // FK to SenderId
        public SenderId SenderId { get; set; }
        public ICollection<Recipient> Recipients { get; set; } = new List<Recipient>();
    }
}
