using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMSHub.APIs.DTOs;
using SMSHub.APIs.Errors;
using SMSHub.Core.Entities;
using SMSHub.Core.Entities.Identity;
using SMSHub.Core.Services;
using SMSHub.Repository.Data;
using System.Security.Claims;

namespace SMSHub.APIs.Controllers
{
    public class SmsController : APIBaseController
    {
        private readonly ISmsService _smsService;
        private readonly SMSHubContext _sMSHubContext;
        private readonly UserManager<AppUser> _userManager;

        public SmsController(ISmsService smsService, SMSHubContext sMSHubContext, UserManager<AppUser>userManager)
        {
            _smsService = smsService;
            _sMSHubContext = sMSHubContext;
            _userManager = userManager;
        }

        [HttpPost("send")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SendSms([FromBody] SmsRequestDto request)
        {
            // Verify sender ID is provided
            if (!request.From.Any())
            {
                return BadRequest("Sender ID must be provided.");
            }

            // Get the current user's Id
            //var userId = User.Claims.First(c => c.Type == "UserID").Value; // Adjust this line based on how you store user identifiers in claims
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
            var userId=user.Id;
            // Attempt to find the corresponding SenderId entity and verify it belongs to the current user
            var senderId = await _sMSHubContext.SenderIds
                  .FirstOrDefaultAsync(sid => sid.SenderIdText == request.From && sid.UserId == userId);

            if (senderId == null)
            {
                return BadRequest("Invalid sender ID or sender ID not assigned to the current user.");
            }

            // Optionally, here you can check if the user's role allows them to send this type of message
            // This requires you to define what "type of message" means in your application and implement the logic accordingly

            // Attempt to send the SMS message to multiple recipients
            var success = await _smsService.SendSmsAsync(request.Recipients, request.Message, senderId.SenderIdText);

            if (success)
            {
                // For each recipient, ensure they are added to the database and linked to the SmsMessage
                var recipientEntities = new List<Recipient>();
                foreach (var recipientPhoneNumber in request.Recipients)
                {
                    var recipient = await _sMSHubContext.Recipients
                                      .FirstOrDefaultAsync(r => r.PhoneNumber == recipientPhoneNumber)
                                   ?? new Recipient { PhoneNumber = recipientPhoneNumber };

                    // Attach recipient to context if it's new
                    if (recipient.Id == 0)
                    {
                        _sMSHubContext.Recipients.Add(recipient);
                    }

                    recipientEntities.Add(recipient);
                }

                // Create the SmsMessage entity
                var smsMessage = new SmsMessage
                {
                    Message = request.Message,
                    SentTime = DateTime.UtcNow,
                    SenderId = senderId, // Link the SmsMessage to the SenderId entity
                    Recipients = recipientEntities // Link the SmsMessage to the recipient entities
                };

                _sMSHubContext.SmsMessages.Add(smsMessage);
                await _sMSHubContext.SaveChangesAsync();

                return Ok("The message is sent successfully.");
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Failed to send SMS."));
            }
        }



    }
}
