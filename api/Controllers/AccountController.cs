using api.Dtos.Account;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };
            var createUser = await _userManager.CreateAsync(user, registerDto.Password);
            
            if (createUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                
                if(roleResult.Succeeded)
                {
                    return Ok("User created");
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            return StatusCode(500, createUser.Errors);
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }
}