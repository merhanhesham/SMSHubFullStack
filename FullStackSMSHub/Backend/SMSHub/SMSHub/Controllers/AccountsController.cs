using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMSHub.APIs.Controllers;
using SMSHub.APIs.DTOs;
using SMSHub.APIs.Errors;
using SMSHub.Core.Entities.Identity;
using SMSHub.Core.Services;
using System.Security.Claims;
using SMSHub.APIs.DTOs;
using SMSHub.APIs.Errors;
using SMSHub.APIs.Extensions;
using SMSHub.Core.Entities.Identity;
using SMSHub.Core.Services;
using Microsoft.EntityFrameworkCore;
using SMSHub.Core.Entities;
using SMSHub.Repository.Data;

namespace SMSHub.APIs.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SMSHubContext _sMSHubContext;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper, RoleManager<IdentityRole> roleManager, SMSHubContext sMSHubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleManager = roleManager;
            _sMSHubContext = sMSHubContext;
        }
        //register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse(400, "Validation failed"));
            }

            var emailExists = await _userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                return BadRequest(new ApiResponse(400, "This email already exists"));
            }

            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400, result.Errors.FirstOrDefault()?.Description));
            }

            // Assign the default role or the specified role
            string roleToAssign = model.Role ?? "CustomerSupport";
            var roleResult = await _userManager.AddToRoleAsync(user, roleToAssign);
            if (!roleResult.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Failed to assign user role"));
            }

            // Here, create and add a new Users record linked to the newly created AppUser
            var newUserProfile = new Users // Assuming Users is your custom entity class
            {
                Id = user.Id, // Link to the ASP.NET Identity user record
                                  // Populate other fields as necessary
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber= model.PhoneNumber,
                //Password=model.Password,
                Role=model.Role
            };
            _sMSHubContext.Users.Add(newUserProfile); // Assuming _context is your DbContext with a DbSet<Users>
            await _sMSHubContext.SaveChangesAsync();

            var returnedUser = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager) // Make sure this method exists and works as expected
            };

            return Ok(returnedUser);
        }

        //login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User == null) { return Unauthorized(new ApiResponse(401)); }
            //takes user and password, checks pass to sign in if true
            var result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);//false>> i don't want to lock acc if pass is false
            if (!result.Succeeded) { return Unauthorized(new ApiResponse(401)); }
            return Ok(new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });


        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);//User is a property inside the controller base
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedUser);
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(MappedAddress);
            //var user = await _userManager.FindByEmailAsync(Email);
            //it won't get the address, bec address is a nav property so it's not loaded
            //so i will make an ext method to get user with address

        }

        [Authorize]
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto OldAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user == null) { return Unauthorized(new ApiResponse(401)); }
            var address = _mapper.Map<AddressDto, Address>(OldAddress);
            address.Id = user.Address.Id;//in order to update on the old address, not create a new object
            user.Address = address;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) { return BadRequest(new ApiResponse(400)); }
            return Ok(OldAddress);
        }
        [HttpGet("emailExists")]
        public async Task<bool> CheckEmailExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

    }
}

/*JWT
 * library i will use to encrypt and decrypt token
 * Jwt>>needs 3 kinds of info>> header(algorithm&type),payload,key(will encrypt through it)
 * 
 */