using Application.Invoices;

using Domain;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ApplicationTests.Invoices

{
    public class DeleteTest : TestBase
    {
        [Fact]
        public async void Should_delete()
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
                Name = "Test",
                Size = 10,
                PurchaseId = id

            };
            context.Photos.Add(testInvoice);
            context.SaveChanges();


            var testPurchase = context.Purchases.Find(id);
            testPurchase.Invoice.Add(testInvoice);

            var sut = new Delete.Handler(context, userAccessor.Object);

            var result = sut.Handle(new Delete.Command { InvoiceId= photoId, PurchaseId=id}, CancellationToken.None).Result;


            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Null(testPurchase.Invoice.FirstOrDefault(x=>x.Id == photoId));

        }

        [Fact]
        public async void Should_Fail_2_delete_If_No_User()
        {

            var context = GetDbContext();
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
                Name = "Test",
                Size = 10,
                PurchaseId = id

            };
            context.Photos.Add(testInvoice);
            context.SaveChanges();


            var testPurchase = context.Purchases.Find(id);
            testPurchase.Invoice.Add(testInvoice);

            var sut = new Delete.Handler(context, userAccessor.Object);
            var result = sut.Handle(new Delete.Command { InvoiceId = photoId, PurchaseId = id }, CancellationToken.None).Result;


            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.NotNull(testPurchase.Invoice.FirstOrDefault(x => x.Id == photoId));
        }
    }
}
