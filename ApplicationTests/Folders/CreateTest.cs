using Application.Core;
using Application.Folders;
using Application.Interfaces;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Folders
{

    public class CreateTest : TestBase
    {
        public  CreateTest()
        {

        }

        [Fact]
        public void Should_Create_Folder()
        {


            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser {Id="1", Email = "test@example.com", UserName= "test", DisplayName="test" });
            context.SaveChangesAsync();

            var folderCommend = new Create.Command
            {
                Folder = new Application.DTOs.FolderWriteDto { Name = "test" }
            };


            var sut = new Create.Handler(context, userAccessor.Object);
            var result = sut.Handle(folderCommend, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

        }
    }
}
