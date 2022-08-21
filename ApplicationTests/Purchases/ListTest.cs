using Application.Purchases;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationTests.Purchases

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

            context.Purchases.Add(new Purchase
            {
                Id = id,
                Name = "Test A",
                Category = "Test A",
                Description = "Test A",
                PaymentMethod = "Test A",
                Price = 0.0,
                PriceInDollar = 0.0,
                PurchaseDate = DateTime.UtcNow.AddDays(-10),
                Reccuring = "Test",
                Seller = "Test",
                Invoice = null,
                UserId = "1",

            });
            context.Purchases.Add(new Purchase
            {
                Id = new Guid(),
                Name = "Test B",
                Category = "Test B",
                Description = "Test B",
                PaymentMethod = "Test B",
                Price = 0.0,
                PriceInDollar = 0.0,
                PurchaseDate = DateTime.UtcNow,
                Reccuring = "Test",
                Seller = "Test",
                Invoice = null,
                UserId = "2",

            });
            context.Purchases.Add(new Purchase
            {
                Id = new Guid(),
                Name = "Test B",
                Category = "Test B",
                Description = "Test B",
                PaymentMethod = "Test B",
                Price = 0.0,
                PriceInDollar = 0.0,
                PurchaseDate = DateTime.UtcNow,
                Reccuring = "Test",
                Seller = "Test",
                Invoice = null,
                UserId = "2",

            });
            context.Purchases.Add(new Purchase
            {
                Id = new Guid(),
                Name = "Test B",
                Category = "Test B",
                Description = "Test B",
                PaymentMethod = "Test B",
                Price = 0.0,
                PriceInDollar = 0.0,
                PurchaseDate = DateTime.UtcNow,
                Reccuring = "Test",
                Seller = "Test",
                Invoice = null,
                UserId = "2",

            });
            context.Purchases.Add(new Purchase
            {
                Id = new Guid(),
                Name = "Test B",
                Category = "Test B",
                Description = "Test B",
                PaymentMethod = "Test B",
                Price = 0.0,
                PriceInDollar = 0.0,
                PurchaseDate = DateTime.UtcNow,
                Reccuring = "Test",
                Seller = "Test",
                Invoice = null,
                UserId = "2",

            });
            context.Purchases.Add(new Purchase
            {
                Id = new Guid(),
                Name = "Test T",
                Category = "Test T",
                Description = "Test t",
                PaymentMethod = "Test T",
                Price = 0.0,
                PriceInDollar = 0.0,
                PurchaseDate = DateTime.UtcNow.AddDays(-10),
                Reccuring = "Test",
                Seller = "Test",
                Invoice = null,
                UserId = "1",

            }); context.Purchases.Add(new Purchase
            {
                Id = new Guid(),
                Name = "Test F",
                Category = "Test F",
                Description = "Test F",
                PaymentMethod = "Test F",
                Price = 0.0,
                PriceInDollar = 0.0,
                PurchaseDate = DateTime.UtcNow.AddDays(-10),
                Reccuring = "Test",
                Seller = "Test",
                Invoice = null,
                UserId = "1",

            });
            context.SaveChanges();


            var sut = new List.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new List.Query { PagedParams = new PurchaseParams { StartDate = DateTime.UtcNow.AddDays(-20)} }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.Equal(3, result.Value.Count);
            Assert.Equal("Test A", result.Value[0].Name);
        }

        //TODO: mopre testing 
    }
}
