using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSHub.Core.Entities
{
    public class Recipient : BaseEntity
    { 
        public string PhoneNumber { get; set; }
        public ICollection<SmsMessage> SmsMessages { get; set; } = new List<SmsMessage>();
    }
}
