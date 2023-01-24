using Microsoft.AspNetCore.Mvc;
using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;

namespace TokonyadiaRestAPII.Services;

public interface IAccountService
{
    Task<UserDto> Register(RegisterDto payload);
    Task<bool> UserExists(string username);
    Task<ActionResult<UserDto>> Login(LoginDto payload);
    
}