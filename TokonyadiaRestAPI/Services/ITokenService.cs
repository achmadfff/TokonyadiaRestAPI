using TokonyadiaRestAPII.Entities;

namespace TokonyadiaRestAPII.Services;

public interface ITokenService
{
    string CreateToken(User user);
}