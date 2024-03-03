using SMSHub.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSHub.Core.Entities
{
    public class SenderId : BaseEntity
    {
            public string UserId { get; set; } 
            public string SenderIdText { get; set; }
            public string Description { get; set; } // Optional
            public bool IsActive { get; set; }
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public ICollection<SmsMessage> SmsMessages { get; set; }
            public ICollection<Users> Users { get; set; }

    }
}
