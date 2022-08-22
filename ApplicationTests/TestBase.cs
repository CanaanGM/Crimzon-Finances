using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Services;
using Application.Core;
using Application.Interfaces;

using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;

using Persistence;

namespace ApplicationTests
{
    public class TestBase
    {
        public Mock<IHttpContextAccessor> httpContextMock; // not really sure for now 

        public Mock<IConfiguration> ConfigurationMock;
        public Mock<UserManager<AppUser>> UserManagerMock;
        public Mock<SignInManager<AppUser>> SignInManagerMock;
        public Mock<TokenService> TokenServiceMock;
        public Mock<IUserAccessor> userAccessor ;
        public readonly IMapper _mapper;
        public AppUser user;

        

        public TestBase()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfiles()));
            _mapper = mockMapper.CreateMapper();

            userAccessor = new Mock<IUserAccessor>();
            userAccessor.Setup(x => x.GetUsername()).Returns("test");
            userAccessor.Setup(x => x.GetUserId()).Returns("1");

            UserManagerMock = new Mock<UserManager<AppUser>>();
            UserManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).Returns(It.IsAny<Task<AppUser>>() );

            SignInManagerMock = new Mock<SignInManager<AppUser>>();
            SignInManagerMock.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), false))
                .Returns(It.IsAny<Task<SignInResult>>());
            
            ConfigurationMock = new Mock<IConfiguration>();
            ConfigurationMock.Setup(x => x[It.IsAny<string>()]).Returns("Is it secret? is it safe?");
           
            user = new AppUser()
            {
                UserName = "test",
                Email = "test@example.com",
                Id = "1",
                DisplayName = "test"
            };

            
            

            //TokenServiceMock = new Mock<TokenService>();
            //TokenServiceMock.Setup(x => x.CreateToken(user)).Returns(It.IsAny<string>());
            //TokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns(new RefreshToken());
            

            
            httpContextMock = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var fakeUserId = "1";
            
            httpContextMock.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>())).Returns(It.IsAny<Claim>());
           // httpContextMock.Setup(x => x.HttpContext.User.FindFirstValue("NameIdentifier")).Returns(It.IsAny<string>());
           
            
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