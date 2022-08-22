using Domain;

namespace Application.Interfaces;

public interface ITokenService
{
    string CreateToken(string UserName, string userId, string userEmail);
    RefreshToken GenerateRefreshToken();
}