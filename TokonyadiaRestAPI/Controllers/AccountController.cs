using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;
using TokonyadiaRestAPII.Services;

namespace TokonyadiaRestAPII.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto payload)
    {
        if (await UserExists(payload.Username)) return BadRequest("UserName Is Already Taken");
        var userResponse = await _accountService.Register(payload);

        CommonResponse<UserDto> response = new CommonResponse<UserDto>
        {
            StatusCode = 201,
            Message = "Successfully Register!",
            Data = userResponse
        };

        return Created("api/account/register", response);
    }
    
    private async Task <bool>UserExists(string username)
    {
        return await _accountService.UserExists(username);
    }

    [HttpPost("login")]
    public async Task <ActionResult<User>>Login(LoginDto loginDto)
    {
        var userResponse = await _accountService.Login(loginDto);
        if (userResponse.Value == null)
        {
            return Unauthorized("Invalid Username or Password!");
        }
        CommonResponse<ActionResult<UserDto>> response = new CommonResponse<ActionResult<UserDto>>
        {
            StatusCode = 200,
            Message = "Successfully Login",
            Data = userResponse
        };
        return Ok(response);
    }
}