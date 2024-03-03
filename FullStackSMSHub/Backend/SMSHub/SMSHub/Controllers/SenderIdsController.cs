using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMSHub.APIs.DTOs;
using SMSHub.APIs.Errors;
using SMSHub.Core.Entities;
using SMSHub.Core.Entities.Identity;
using SMSHub.Core.Repositories;
using SMSHub.Repository.Data;
using System.Security.Claims;

namespace SMSHub.APIs.Controllers
{
    public class SenderIdsController : APIBaseController
    {
        private readonly SMSHubContext _context;
        private readonly IGenericRepository<SenderId> _senderIdRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        //private readonly IGenericRepository<Users> _usersRepo;

        public SenderIdsController(SMSHubContext context, IGenericRepository<SenderId> SenderIdRepo, IMapper mapper, UserManager<AppUser>userManager)
        {
            _context = context;
            _senderIdRepo = SenderIdRepo;
            _mapper = mapper;
            _userManager = userManager;
          //  _usersRepo = UsersRepo;
        }

        //admin role
        // Endpoint methods go here
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SenderId>>> GetSenderIds()
        {
             var senderIds = await _senderIdRepo.GetAllAsync();
             return Ok(senderIds); 
        }

        //adminrole
        [HttpGet("{id}")]
        public async Task<ActionResult<SenderId>> GetSenderId(int id)
        {
            var senderId = await _senderIdRepo.GetByIdAsync(id);
            if (senderId == null)
            {
                return NotFound(new ApiResponse(404, "SenderId Not Found"));
            }
            return Ok(senderId);
        }

        //adminrole
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> PostSenderId([FromBody] CreateSenderIdDto senderIdDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }

            // Check if a SenderId with the provided SenderIdText already exists for this user
            var existingSenderId = await _context.SenderIds
                .FirstOrDefaultAsync(sid => sid.SenderIdText == senderIdDto.SenderIdText && sid.UserId == user.Id);

            if (existingSenderId != null)
            {
                // If a SenderId with the same SenderIdText for this user already exists, return a conflict response
                return Conflict(new ApiResponse(409, "SenderId already exists."));
            }

            // Map the DTO to your entity
            var mappedSenderId = _mapper.Map<SenderId>(senderIdDto);
            // Set the UserId property of the SenderId entity
            mappedSenderId.UserId = user.Id;

            // Add the new SenderId to the database
            await _context.SenderIds.AddAsync(mappedSenderId);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse(200, "SenderId created successfully."));
        }

        //adminrole
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> PutSenderId(int id, [FromBody] UpdateSenderIdDto senderId)
        {
            if (id != senderId.Id)
            {
                return BadRequest();
            }
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
            try
            {
                senderId.Id = id;
                // Update the SenderId using the GenericRepository's Update method
                senderId.UserId = user.Id;
                var mappedSenderId = _mapper.Map<SenderId>(senderId);
                _senderIdRepo.Update(mappedSenderId);
                await _context.SaveChangesAsync(); // Assuming your repository has a SaveChangesAsync method
                return Ok("senderId updated successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.SenderIds.Any(e => e.Id == id))
                {
                    return NotFound(new ApiResponse(404, "erorrrr"));
                }
                else
                {
                    throw;
                }
            }
            //return NoContent();
        }

        //admin role
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteSenderId(int id)
        {
            var senderId = await _senderIdRepo.GetByIdAsync(id);
            if (senderId == null)
            {
                return NotFound(new ApiResponse(404, "SenderId Not Found"));
            }

            try
            {
                _context.SenderIds.Remove(senderId);
                await _context.SaveChangesAsync();
                return Ok("senderId deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the senderId: {ex.Message}");
            }
        }
         
        //admin role
        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        //admin role
        [HttpPost("AssignSenderIds")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> AssignSenderIdsToUser([FromBody] AssignSenderIdsDto dto)
        {
            // Find the user by ID
            var user = await _context.Users
                .Include(u => u.SenderIds) // Make sure to include SenderIds to update them
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);

            if (user == null)
            {
                return NotFound($"User with ID {dto.UserId} was not found.");
            }

            // Find all sender IDs that match the provided SenderIdTexts
            var senderIdsToAssign = await _context.SenderIds
                .Where(sid => dto.SenderIdTexts.Contains(sid.SenderIdText) && sid.IsActive)
                .ToListAsync();

            // Ensure all senderIdTexts correspond to an active SenderId
            if (senderIdsToAssign.Count != dto.SenderIdTexts.Count)
            {
                return NotFound("One or more SenderIds were not found or are not active.");
            }

            // Assign the sender IDs to the user
            user.SenderIds = senderIdsToAssign;

            await _context.SaveChangesAsync();

            return Ok($"SenderIds successfully assigned to user {dto.UserId}.");
        }

        // Endpoint to get SenderIds assigned to the logged-in user
        // Endpoint to get SenderIds assigned to the logged-in user
        [HttpGet("assignedSenderIds")]
        public async Task<IActionResult> GetAssignedSenderIds()
        {
            // Get the current user's Id
            //var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404, "Target user not found."));
            }
            var userId = user.Id;
            // Query the SenderIds assigned to the current user
            var assignedSenderIds = await _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.SenderIds)
                .Select(sid => new
                {
                    sid.SenderIdText,
                    sid.Description,
                    sid.IsActive,
                    sid.CreatedAt
                })
                .ToListAsync();

            if (!assignedSenderIds.Any())
            {
                return NotFound("No Sender IDs found for the current user.");
            }

            return Ok(assignedSenderIds);
        }

        // Endpoint to get SmsMessages along with SenderId (from) and Recipients
        [HttpGet("GetSmsDetails")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetSmsDetails()
        {
            var smsDetails = await _context.SmsMessages
                .Select(sms => new
                {
                    Message = sms.Message,
                    From = sms.SenderId.SenderIdText,
                    SentTime = sms.SentTime,
                    Recipients = sms.Recipients.Select(r => r.PhoneNumber).ToList()
                })
                .ToListAsync();

            if (!smsDetails.Any())
            {
                return NotFound("No SMS messages found.");
            }

            return Ok(smsDetails);
        }
        
        [HttpGet("GetTemplates")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<Template>>> GetTemplates()
        {
            return await _context.Templates.ToListAsync();
        }
        
    }

}


