using Application.Invoices;
using Application.DTOs;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApplicationTests.Invoices

{
    public class EditTest : TestBase
    {
        [Fact]
        public void Should_Be_Able_To_Edit()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");
            Guid photoId = new Guid("CFC0264F-2BD3-4066-81A9-8256E34F3141");
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
                Invoice = new List<Photo>(),
                UserId = "1",

            });

            context.SaveChanges();

            byte[] filebytes = Encoding.UTF8.GetBytes("MockImages/osaka_stares_into_ur_soul.jpg");
            var testInvoice = new Photo
            {
                Id = photoId,
                Bytes = filebytes,
                Description = "Test",
                FileExtension = "test/jpg",
                Name = "test.jpg",
                Size = 10,
                PurchaseId = id

            };
            context.Photos.Add(testInvoice);

            context.SaveChanges();

            var testPurchase = context.Purchases.Find(id);



            var file = new FormFile(
                baseStream: new MemoryStream(filebytes),
                baseStreamOffset: 0,
                length: filebytes.Length,
                name: "MockData",
                fileName: "osaka_stares_into_ur_soul.jpg"
            )
            {
                Headers = new HeaderDictionary(),
                ContentType = "test/png"
            };


            var alteredInvoice = new PhotoUpdateDto { File = file }; 

            var sut = new Edit.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new Edit.Command { PurchaseId = id,InvoiceId= photoId, Invoice=alteredInvoice }, CancellationToken.None).Result;


            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(testPurchase.Invoice);
            Assert.Equal("osaka_stares_into_ur_soul.jpg", testPurchase.Invoice.ToArray()[0].Name);

        }
    }
}
