using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Services;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationTests.Services;

public class TokenServiceTest : TestBase
{
    [Fact]
    public void Should_Create_Token()
    {

        var token = GenToken(user);
        
        var sut = new TokenService(ConfigurationMock.Object);

        var res = sut.CreateToken(user);
        
        Assert.NotEmpty(res);
        Assert.Equal(token, res);
    }

    [Fact]
    public void Should_Generate_RefreshToken()
    {
        var sut = new TokenService(ConfigurationMock.Object);
        var res = sut.GenerateRefreshToken();
        
        Assert.NotNull(res);
        Assert.NotEqual(res, sut.GenerateRefreshToken());
    }

}