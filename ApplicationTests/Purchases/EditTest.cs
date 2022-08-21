using Application.Purchases;
using Application.DTOs;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Purchases

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

            context.Purchases.Add(new Purchase
            {
                Id = id,
                Name = "Test",
                Category = "Test",
                Description = "Test",
                PaymentMethod = "Test",
                Price = 0.0,
                PriceInDollar = 0.0,
                PurchaseDate = DateTime.UtcNow,
                Reccuring = "Test",
                Seller = "Test",
                Invoice = null,
                UserId = "1",

            });

            context.SaveChanges();

            var purchaseInfo = new PurchaseUpdateDto 
            {
                Name = "Canaan",
                Category = "Human",
                Description = "What am i doing ?",
                PaymentMethod = "Test",
                Price = 0.0,
                PriceInDollar = 0.0,
                PurchaseDate = DateTime.UtcNow,
                Reccuring = "Test",
                Seller = "Test"
            };
            
            var sut = new Edit.Handler(context, _mapper, userAccessor.Object);

            var result = sut.Handle(new Edit.Command { Id = id, Purchase = purchaseInfo }, CancellationToken.None).Result;

            var testPurr = context.Purchases.Find(id);
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(id, testPurr.Id);
            Assert.Equal("Canaan", testPurr.Name);
            Assert.Equal("1", testPurr.UserId);


        }
    }
}
