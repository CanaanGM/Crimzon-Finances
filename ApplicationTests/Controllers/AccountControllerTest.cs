using API.Controllers;
using Application.DTOs;

namespace ApplicationTests.Controllers;

public class AccountControllerTest : TestBase
{

    public void Should_Login_Existing_User()
    {
        var context = GetDbContext();
        context.Users.AddAsync(user);
        
        context.SaveChangesAsync();

        var sut = new AccountController(UserManagerMock.Object, SignInManagerMock.Object, TokenServiceMock.Object);
        var result = sut.Login(new AppLoginDto() {Email = user.Email, Password = "Pa$$w0rd!"});
        
        Assert.NotNull(result);

        
    }

    public async void Should_Create_User_Success()
    {
        var regModel = new AppRegisterDto
        {
            Email = "Nero@example.com",
            Password = "Pa$$w0rd!",
            DisplayName = "Nero",
            UserName = "Nero"
        };

        var controller =
            new AccountController(UserManagerMock.Object, SignInManagerMock.Object, TokenServiceMock.Object);

        var result = await  controller.Register(regModel);
        
        Assert.NotNull(result);
    }
}