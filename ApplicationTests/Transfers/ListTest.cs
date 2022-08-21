using Application.Transfers;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationTests.Transfers

{
    public class ListTest : TestBase
    {
        [Fact]
        public void Should_Return_List()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");


            context.Transfers.Add(new Transfer
            {
                Id = id,
                Amount = 0.0,
                Name = "Doom",
                Reciever = "Test",
                FromBank = "Test",
                FromAccount = "Test",
                Description = "Test",
                DateWasMade = DateTime.UtcNow.AddDays(-3),
                TransferType = "Test",
                RecieverAccount = "Test",
                UserId = "1",


            });
            context.Transfers.Add(new Transfer
            {
                Id = new Guid(),
                Amount = 0.0,
                Name = "Lambda",
                Reciever = "Test",
                FromBank = "Test",
                FromAccount = "Test",
                Description = "Test",
                DateWasMade = DateTime.UtcNow.AddDays(-3),
                TransferType = "Test",
                RecieverAccount = "Test",
                UserId = "1",


            });
            context.Transfers.Add(new Transfer
            {
                Id = new Guid(),
                Amount = 0.0,
                Name = "Susanoo",
                Reciever = "Test",
                FromBank = "Test",
                FromAccount = "Test",
                Description = "Test",
                DateWasMade = DateTime.UtcNow,
                TransferType = "Test",
                RecieverAccount = "Test",
                UserId = "2",


            });

            context.SaveChanges();


            var sut = new List.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new List.Query { PagedParams=new TransferParams { StartDate = DateTime.UtcNow.AddDays(-10)} }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal("Doom", result.Value[0].Name);
        }

        //TODO: mopre testing 
    }
}
