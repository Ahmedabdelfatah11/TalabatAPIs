using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using TalabatAPIs.Dtos;
using TalabatAPIs.Errors;

namespace TalabatAPIs.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
               SignInManager<ApplicationUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                Token = "New Token"
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
                DisplayName=user.DisplayName,
                Email = user.Email,
                Token="This Will Be A Token"
            });

        }
    }
}
