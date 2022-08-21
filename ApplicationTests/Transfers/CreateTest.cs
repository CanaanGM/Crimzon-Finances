using Application.Transfers;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Transfers
{
    public class CreateTest : TestBase
    {
        [Fact]
        public void Should_Create()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            var transfer = new Create.Command
            {
                Transfer = new Application.DTOs.TransferWriteDto
                {
                    Amount = 0.0,
                    Name = "Test",
                    Reciever = "Test",
                    FromBank = "Test",
                    FromAccount = "Test",
                    Description = "Test",
                    DateWasMade = DateTime.UtcNow,
                    TransferType = "Test",
                    RecieverAccount = "Test",
                }
            };


            var sut = new Create.Handler(context,_mapper, userAccessor.Object);
            var result = sut.Handle(transfer, CancellationToken.None).Result;
            
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

        }

        [Fact]
        public void Should_Fail_With_No_User()
        {
            var context = GetDbContext();

            var transfer = new Create.Command
            {
                Transfer = new Application.DTOs.TransferWriteDto
                {
                    Amount = 0.0,
                    Name = "Test",
                    Reciever = "Test",
                    FromBank = "Test",
                    FromAccount = "Test",
                    Description = "Test",
                    DateWasMade = DateTime.UtcNow,
                    TransferType = "Test",
                    RecieverAccount = "Test",
                }
            };


            var sut = new Create.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(transfer, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }
    }
}
