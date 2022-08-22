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
        
        var sut = TokenServiceMock.Object;

        var res = sut.CreateToken(user.UserName, user.Id, user.Email);
        
        Assert.NotEmpty(res);
        Assert.Equal(token, res);
    }

    [Fact]
    public void Should_Generate_RefreshToken()
    {
        var sut =  TokenServiceMock.Object;
        var res = sut.GenerateRefreshToken();
        
        Assert.NotNull(res);
        Assert.Equal(res, sut.GenerateRefreshToken());
    }

}