using Application.Purchases;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Purchases

{
    public class CreateTest : TestBase
    {
        [Fact]
        public void Should_Create()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            var purchase = new Create.Command
            {
                Purchase = new Application.DTOs.PurchaseWriteDto
                {
                    Name = "Test",
                    Category = "Test",
                    Description = "Test",
                    PaymentMethod = "Test",
                    Price = 0.0,
                    PriceInDollar = 0.0,
                    PurchaseDate = DateTime.UtcNow,
                    Reccuring = "Test",
                    Seller = "Test",

                }
            };


            var sut = new Create.Handler(context,  userAccessor.Object, _mapper);
            var result = sut.Handle(purchase, CancellationToken.None).Result;
            
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

        }

        [Fact]
        public void Should_Fail_With_No_User()
        {
            var context = GetDbContext();

            var dept = new Create.Command
            {
                Purchase = new Application.DTOs.PurchaseWriteDto
                {
                    Name = "Test",
                    Category = "Test",
                    Description = "Test",
                    PaymentMethod = "Test",
                    Price = 0.0,
                    PriceInDollar = 0.0,
                    PurchaseDate = DateTime.UtcNow,
                    Reccuring = "Test",
                    Seller = "Test",

                }
            };


            var sut = new Create.Handler(context, userAccessor.Object, _mapper);
            var result = sut.Handle(dept, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }
    }
}
