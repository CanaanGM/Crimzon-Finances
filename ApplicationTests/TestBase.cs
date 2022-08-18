using Application.Core;
using Application.Interfaces;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Moq;

using Persistence;

namespace ApplicationTests
{
    public class TestBase
    {
        public Mock<IUserAccessor> userAccessor ;
        public readonly IMapper _mapper;


        public TestBase()
        {
            var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfiles()));
            _mapper = mockMapper.CreateMapper();

            userAccessor = new Mock<IUserAccessor>();
            userAccessor.Setup(x => x.GetUsername()).Returns("test");
            userAccessor.Setup(x => x.GetUserId()).Returns("1");
        }

        public DataContext GetDbContext()
        {
            

            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString()); 

            var dbContext = new DataContext(builder.Options);

            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        
    }
}