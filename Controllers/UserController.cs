using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sq016FirstApi.Data.Entities;
using Sq016FirstApi.DTOs;
using System.Globalization;
using System.Security.Claims;

namespace Sq016FirstApi.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody]AddUserDto model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email
                };

                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if(identityResult.Succeeded)
                {
                    var result = await _userManager.AddToRoleAsync(user, "regular");
                    if (!result.Succeeded)
                    {
                        foreach (var err in result.Errors)
                        {
                            ModelState.AddModelError(err.Code, err.Description);
                        }
                        return BadRequest(ModelState);
                    }

                    var userToReturn = new ReturnUserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                    };

                    return Ok(userToReturn);
                }

                foreach(var err in identityResult.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }

            }
            return BadRequest(ModelState);
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.ToList();
            var usersToReturn = new List<ReturnUserDto>();
            if (users.Any())
            {
                foreach (var user in users)
                {
                    usersToReturn.Add(new ReturnUserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                    });
                }
            }

            return Ok(usersToReturn);
        }

        [HttpGet("single/{id}")]
        public async Task<IActionResult> GetSingle(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                
                var userToReturn = new ReturnUserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };

                return Ok(userToReturn);

            }

            return NotFound($"No user was found with id: {id}");
        }

        [Authorize(Roles ="admin")]
        [HttpPost("add-user-role")]
        public async Task<IActionResult> AddUserRole([FromBody] AddUserRoleDto model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return NotFound($"Could not find user with Id: {model.UserId}");

                var result = await _userManager.AddToRoleAsync(user, model.Role);
                if (!result.Succeeded)
                {
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Code, err.Description);
                    }
                }

                return Ok("Role added to user!");

            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles ="regular")]
        [HttpPost("get-user-roles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Could not find user with Id: {userId}");

            var userRoles = await _userManager.GetRolesAsync(user);

            return Ok(userRoles);
        }

        [Authorize(Roles="admin")]
        [HttpPost("add-claim-to-user")]
        public async Task<IActionResult> AddClaimsToUser([FromBody]AddUserClaimsDto model)
        {
            if (ModelState.IsValid)
            {
                // if user already has these claims

                // Make sure that the logged in user cannot add claims to itself
                var loggedInUser = await _userManager.FindByNameAsync(User?.Identity?.Name);
                //if (loggedInUser != null && loggedInUser.Id != model.UserId)
                //{

                //}
                //return BadRequest($"Loggedin is denied from assigning claims to self.");

                var user = await _userManager.FindByIdAsync(model.UserId);

                var claimList = new List<Claim>();

                foreach (var item in model.ClaimsToAdd)
                {
                    claimList.Add(new Claim(item.Key, item.Value));
                }

                if (user != null)
                {
                    var result = await _userManager.AddClaimsAsync(user, claimList);
                    if (result.Succeeded)
                    {
                        return Ok("Claims added to user.");
                    }

                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError(err.Code, err.Description);
                    }
                    return BadRequest(ModelState);
                }

                return BadRequest($"User with id {model.UserId} was not found!");
            }

            return BadRequest(ModelState);
        }
    }
}
