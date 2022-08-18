using Application.Core;
using Application.Errors;
using Application.Folders;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Folders
{
    public class DetailsTest : TestBase
    {

        public DetailsTest()
        {

        }

        [Fact]
        public void Should_Get_Folder_Details()
        {
            var context = GetDbContext();

            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            context.Folders.Add(new Domain.Folder { Id = id, Name = "test" , UserId = "1"});

            context.Folders.Add(new Domain.Folder { Id = new Guid(), Name = "test1", UserId = "2" });
            context.SaveChanges();

            var sut = new Details.Handler(context,_mapper, userAccessor.Object);
            var result = sut.Handle(new Details.Query { Id = id }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(id, result.Value.Id);

        }

        [Fact]
        public async Task Should_Return_404_If_Folder_Not_Found()
        {
            var context = GetDbContext();

            var sut = new Details.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new Details.Query { Id = new Guid() }, CancellationToken.None);

            await Assert.ThrowsAsync<RestException>(() => result);
        }
    }
}
