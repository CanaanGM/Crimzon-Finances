using Application.Folders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Folders
{
    public class DeleteTest : TestBase
    {
        [Fact]
        public async void Should_delete_folder()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");


            await context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            await context.SaveChangesAsync();

            context.Folders.Add(new Domain.Folder { Id = id, Name = "test", UserId = "1" });
            await context.SaveChangesAsync();


            var sut = new Delete.Handler(context, userAccessor.Object);
            var result = sut.Handle(new Delete.Command { Id = id }, CancellationToken.None).Result;
            Assert.True(result.IsSuccess);
            Assert.Null(await context.Folders.FindAsync(id));
        }
    }
}
