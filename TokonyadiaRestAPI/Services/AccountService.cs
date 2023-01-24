using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;
using TokonyadiaRestAPII.Repositories;

namespace TokonyadiaRestAPII.Services;

public class AccountService : IAccountService
{
    private readonly IRepository<User> _userRepository;
    private readonly IPersistence _persistence;
    private readonly ITokenService _tokenService;

    public AccountService(IRepository<User> userRepository, IPersistence persistence, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _persistence = persistence;
        _tokenService = tokenService;
    }

    public async Task<UserDto> Register(RegisterDto payload)
    {
        
        var hmac = new HMACSHA512();

        var user = new User
        {
            UserName = payload.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload.Password)),
            PasswordSalt= hmac.Key,
        };
        
        var entry = await _userRepository.SaveAsync(user);
        await _persistence.SaveChangesAsync();
        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    public async Task<bool> UserExists(string username)
    {
        return await _userRepository.AnyAsync(x => x.UserName == username.ToLower());
    }

    public async Task<ActionResult<UserDto>> Login(LoginDto payload)
    {
        var user = await _userRepository.Find(x => x.UserName == payload.Username);

        if (user == null) return new UnauthorizedResult();

        var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload.Password));

        for(int i=0;i<computedHash.Length;i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return new UnauthorizedResult();
        }

        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }
}