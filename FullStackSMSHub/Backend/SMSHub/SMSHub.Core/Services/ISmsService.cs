using SMSHub.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMSHub.Core.Services
{
    public interface ISmsService
    {
        // Updated SendSmsAsync to accept multiple recipients and a single sender ID
        // Assuming each call to this method uses one sender ID but can target multiple recipients
        Task<bool> SendSmsAsync(ICollection<string> to, string message, string from);

        // The DefaultFromNumber property can remain if you want a default sender ID available
        string DefaultFromNumber { get; }

        // Removed the RateLimitAsync method comment as it was optional and not fully defined
        // If rate limiting is needed, consider implementing it within the SendSmsAsync method or as part of your service logic
    }
}
