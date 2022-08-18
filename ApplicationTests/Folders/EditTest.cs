using Application.DTOs;
using Application.Folders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Folders
{
    public class EditTest : TestBase
    {
        [Fact]
        public void Should_Be_Able_To_Edit_Folder()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            context.Folders.Add(new Domain.Folder { Id = id, Name = "test", UserId = "1" });
            context.SaveChanges();


            var folderData = new FolderWriteDto { Name = "Test Tea" };
            var sut = new Edit.Handler(context, _mapper, userAccessor.Object);

            var result = sut.Handle(new Edit.Command { Id = id, Folder = folderData }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            Assert.Equal("Test Tea", result.Value.Name);

        }
    }
}
