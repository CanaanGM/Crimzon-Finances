using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Services;
using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

using Persistence;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ApplicationTests
{
    public class TestBase
    {
        public Mock<IHttpContextAccessor> httpContextAccessorMock; // not really sure for now 

        public Mock<IConfiguration> ConfigurationMock;
        public Mock<UserManager<AppUser>> UserManagerMock;
        public Mock<SignInManager<AppUser>> SignInManagerMock;
        public Mock<ITokenService> TokenServiceMock;
        public Mock<IUserAccessor> userAccessor ;
        public Mock<IAccountController> AccountControllerMock;
        public readonly IMapper _mapper;
        public AppUser user;

        

        public TestBase()
        {
            
            user = new AppUser()
            {
                UserName = "test",
                Email = "test@example.com",
                Id = "1",
                DisplayName = "test"
            };
            
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfiles()));
            _mapper = mockMapper.CreateMapper();

            httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeUserId = "1";
            
            httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>())).Returns(It.IsAny<Claim>());
            // httpContextMock.Setup(x => x.HttpContext.User.FindFirstValue("NameIdentifier")).Returns(It.IsAny<string>());
            
            userAccessor = new Mock<IUserAccessor>();
            userAccessor.Setup(x => x.GetUsername()).Returns("test");
            userAccessor.Setup(x => x.GetUserId()).Returns("1");
            

            UserManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(),null, null, null, null, null, null, null, null );
            UserManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .Returns( Task.FromResult<AppUser>(user)) ;

            SignInManagerMock = new Mock<SignInManager<AppUser>>(
                UserManagerMock.Object,
                httpContextAccessorMock.Object,
                Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
                null,
                null,
                null,
                null
            );
            SignInManagerMock.Setup(x => 
                    x.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false))
                .Returns(It.IsAny<Task<SignInResult>>());

            SignInManagerMock.Setup(x => 
                    x.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false))
                .Returns(Task.FromResult<SignInResult>(SignInResult.Success));
            
            ConfigurationMock = new Mock<IConfiguration>();
            ConfigurationMock.Setup(x => x[It.IsAny<string>()]).Returns("Is it secret? is it safe?");
           
 

            
            

            TokenServiceMock = new Mock<ITokenService>();
            TokenServiceMock.Setup(x => x.CreateToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GenToken(user));
            TokenServiceMock.Setup(x => x.GenerateRefreshToken())
                .Returns(new RefreshToken(){AppUser = user, Token = GenToken(user),Expires = DateTime.UtcNow, Revoked = DateTime.UtcNow});



            AccountControllerMock = new Mock<IAccountController>();
            AccountControllerMock.Setup(x => x.SetRefreshToken(It.IsAny<AppUser>())).Returns(Task.CompletedTask);
            AccountControllerMock.Setup(c => c.CheckIfUserPropsAreTaken(It.IsAny<AppRegisterDto>()))
                .Returns(Task.FromResult<Tuple<string,string>>(new ("Success","nonesense")));

        }

        public DataContext GetDbContext()
        {
            

            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString()); 

            var dbContext = new DataContext(builder.Options);

            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        public string GenToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Is it secret? is it safe?"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}