using Application.Transfers;
using Application.DTOs;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Transfers

{
    public class EditTest : TestBase
    {
        [Fact]
        public void Should_Be_Able_To_Edit()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            context.Transfers.Add(new Transfer
            {
                Id = id,
                Amount = 0.0,
                Name = "Test",
                Reciever = "Test",
                FromBank = "Test",
                FromAccount = "Test",
                Description = "Test",
                DateWasMade = DateTime.UtcNow,
                TransferType = "Test",
                RecieverAccount = "Test",
                UserId = "1",


            });

            context.SaveChanges();

            var mrTInfo = new TransferWriteDto 
            {
                Amount = 0.0,
                Name = "mrT",
                Reciever = "Test",
                FromBank = "Test",
                FromAccount = "Test",
                Description = "I pity the fool",
                DateWasMade = DateTime.UtcNow,
                TransferType = "Test",
                RecieverAccount = "Test",
            };
            
            var sut = new Edit.Handler(context, _mapper, userAccessor.Object);

            var result = sut.Handle(new Edit.Command { Id = id, Transfer = mrTInfo }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            Assert.Equal("mrT", result.Value.Name);
            Assert.Equal("I pity the fool", result.Value.Description);


        }
    }
}
