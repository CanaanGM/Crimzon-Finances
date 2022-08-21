﻿using Application.Invoices;

using Domain;

using Microsoft.AspNetCore.Http;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Invoices
{
    public class CreateTest : TestBase
    {
        [Fact]
        public void Should_Create()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");
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

            var file = new FormFile(
                baseStream: new MemoryStream(filebytes),
                baseStreamOffset: 0,
                length: filebytes.Length,
                name: "MockData",
                fileName: "osaka_stares_into_ur_soul.jpg"
                )
            { Headers = new HeaderDictionary(), ContentType = "test/jpg" };


            var invoice = new Create.Command
            {
                Invoice = new Application.DTOs.PhotoWriteDto
                {
                    Files = new List<IFormFile> { file }
                }
                ,PurchaseId = id,
            };


            var sut = new Create.Handler(context, userAccessor.Object);
            var result = sut.Handle(invoice, CancellationToken.None).Result;

            var testPurchase = context.Purchases.Find(id);

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(testPurchase.Invoice);
            Assert.Equal("osaka_stares_into_ur_soul.jpg", testPurchase.Invoice.ToArray()[0].Name);

        }

        [Fact]
        public void Should_Fail_With_No_User()
        {
            var context = GetDbContext();


            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");
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

            var file = new FormFile(
                baseStream: new MemoryStream(filebytes),
                baseStreamOffset: 0,
                length: filebytes.Length,
                name: "MockData",
                fileName: "osaka_stares_into_ur_soul.jpg"
                )
            { Headers = new HeaderDictionary(), ContentType = "test/jpg" };


            var invoice = new Create.Command
            {
                Invoice = new Application.DTOs.PhotoWriteDto
                {
                    Files = new List<IFormFile> { file }
                }
                ,
                PurchaseId = id,
            };


            var sut = new Create.Handler(context, userAccessor.Object);
            var result = sut.Handle(invoice, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public void Should_Fail_With_No_Purchase()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();



            byte[] filebytes = Encoding.UTF8.GetBytes("MockImages/osaka_stares_into_ur_soul.jpg");

            var file = new FormFile(
                baseStream: new MemoryStream(filebytes),
                baseStreamOffset: 0,
                length: filebytes.Length,
                name: "MockData",
                fileName: "osaka_stares_into_ur_soul.jpg"
                )
            { Headers = new HeaderDictionary(), ContentType = "test/jpg" };




            var invoice = new Create.Command
            {
                Invoice = new Application.DTOs.PhotoWriteDto
                {
                    Files = new List<IFormFile> { file }
                }
            };


            var sut = new Create.Handler(context, userAccessor.Object);
            var result = sut.Handle(invoice, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }
    }
}
