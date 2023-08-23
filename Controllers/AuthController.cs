using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Sq016FirstApi.Data.Entities;
using Sq016FirstApi.DTOs;
using Sq016FirstApi.Services;

namespace Sq016FirstApi.Controllers
{
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        public AuthController(IAuthService authService, UserManager<AppUser> userManager)
        {
            _authService= authService;
            _userManager= userManager;
        }


        //[HttpGet("get-token")]
        //public IActionResult GetToken()
        //{
        //    var token = _authService.GenerateJWT();

        //    return Ok(token);
        //}


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginDto model)
        {
            var response = new ResponseObject<object, object>();
            response.Code = 400;
            response.Message = "Failed";
            response.Data = null;
            response.Status = false;

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await _authService.Login(user, model.Password);
                    if(result != null && result.Succeeded)
                    {
                        var userToReturn = new ReturnUserDto
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                        };

                        var userroles = await _userManager.GetRolesAsync(user);
                        var userclaims = await _userManager.GetClaimsAsync(user);

                        var token = _authService.GenerateJWT(user, userroles, userclaims);

                        response.Code = 200;
                        response.Message = "Succesful";
                        response.Data = new { user = userToReturn, token = token };
                        response.Status = true;
                        return Ok(response);
                    }

                    response.Errors = "Invaid Credentials";
                    return BadRequest(response);
                }
                response.Errors = "Invaid Credentials";
                return BadRequest(response);
            }
            response.Errors = ModelState;
            return BadRequest(response);
        }
    }
}
