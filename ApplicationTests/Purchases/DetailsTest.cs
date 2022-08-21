using Application.Purchases;
using Application.Errors;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Purchases

{
    public class DetailsTest : TestBase
    {
        [Fact]
        public void Should_Get_Details()
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

            var sut = new Details.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new Details.Query { Id = id }, CancellationToken.None).Result;

            Assert.NotNull(result.Value);
            Assert.True(result.IsSuccess);
            Assert.Equal(id, result.Value.Id);

        }

        [Fact]
        public async Task Should_Return_404_If_Not_Found()
        {
            var context = GetDbContext();

            var sut = new Details.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new Details.Query { Id = new Guid() }, CancellationToken.None);

            await Assert.ThrowsAsync<RestException>(() => result);
        }

    }
}
