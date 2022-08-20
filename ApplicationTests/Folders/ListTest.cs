using Application.Folders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Folders
{
    public class ListTest : TestBase
    {
        [Fact]
        public void List_Should_Return_List_Of_Folders()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            context.Folders.Add(new Domain.Folder { Id = new Guid(), Name = "test - A", UserId = "1" });
            context.Folders.Add(new Domain.Folder { Id = new Guid(), Name = "test - B", UserId = "1" });
            context.Folders.Add(new Domain.Folder { Id = new Guid(), Name = "test - C", UserId = "1" });
            context.Folders.Add(new Domain.Folder { Id = new Guid(), Name = "test - D", UserId = "5" });
            context.Folders.Add(new Domain.Folder { Id = new Guid(), Name = "test - E", UserId = "3" });
            context.Folders.Add(new Domain.Folder { Id = new Guid(), Name = "test - F", UserId = "2" });
            context.SaveChanges();

            var sut = new List.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new List.Query { /*No params for Folders ;=; */ }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.Equal(3, result.Value.Count);
            Assert.Equal("test - A", result.Value[0].Name);



        }
    }
}
