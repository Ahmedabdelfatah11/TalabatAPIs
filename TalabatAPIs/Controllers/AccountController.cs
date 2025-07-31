using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using TalabatAPIs.Dtos;
using TalabatAPIs.Errors;
using TalabatAPIs.Extensions;

namespace TalabatAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager,
               SignInManager<ApplicationUser> signInManager,
               IAuthService authService,
               IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return Unauthorized(new ApiResponse(401, "Invalid Login"));
            }
            var Result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!Result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401, "Invalid Login"));
            }
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email,
                PhoneNumber = model.Phone
            };
            var Result = await _userManager.CreateAsync(user, model.Password);
            if (!Result.Succeeded)
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = Result.Errors.Select(e => e.Description)
                });
            }
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user?.DisplayName ?? string.Empty,
                Email = user?.Email ?? string.Empty,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> getCurrentAddress()
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            return Ok(_mapper.Map<AddressDto>(user.Address));
        } 
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<Address>> UpdateUserAddress(AddressDto address)
        {
            var UpdatedAddress = _mapper.Map<Address>(address);
            var user = await _userManager.FindUserWithAddressAsync(User);
            UpdatedAddress.Id = user.Address.Id;
            user.Address = UpdatedAddress;
            var result =await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                return BadRequest(new ApiValidationErrorResponse()
                {
                    Errors = result.Errors.Select(e => e.Description)
                });
            }
            return Ok(address);
        }
    }
}
