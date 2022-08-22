using Application.Purchases;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace ApplicationTests.Purchases

{
    public class CreateTest : TestBase
    {
        [Fact]
        public void Should_Create()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser
                { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
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


            var sut = new Create.Handler(context, userAccessor.Object, _mapper);
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


        [Fact]
        public void Should_Generate_Photo_From_Request()
        {
            var context = GetDbContext();

            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");
            context.Users.AddAsync(new Domain.AppUser
                { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            var purchase = new Purchase()
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
                Invoice = new List<Photo>(),
                UserId = "1",
            };

            byte[] filebytes = Encoding.UTF8.GetBytes("MockImages/osaka_stares_into_ur_soul.jpg");

            var file = new FormFile(
                    baseStream: new MemoryStream(filebytes),
                    baseStreamOffset: 0,
                    length: filebytes.Length,
                    name: "MockData",
                    fileName: "osaka_stares_into_ur_soul.jpg"
                )
                { Headers = new HeaderDictionary(), ContentType = "test/jpg" };


            var invoice = new Application.DTOs.PhotoWriteDto
            {
                Files = new List<IFormFile> { file }
            };

            var sut = new Create.Handler(context, userAccessor.Object, _mapper);
            var result = sut.GeneratePhotosFromRequest(purchase, invoice).Result;
            
            Assert.NotNull(result);
            Assert.Equal("osaka_stares_into_ur_soul.jpg", result[0].Name);
            Assert.Equal("test/jpg", result[0].FileExtension);
        }
    }
}
