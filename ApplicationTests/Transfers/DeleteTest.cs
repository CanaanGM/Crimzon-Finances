﻿using Application.Transfers;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Transfers

{
    public class DeleteTest : TestBase
    {
        [Fact]
        public async void Should_delete()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");


            await context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            await context.SaveChangesAsync();

            context.Transfers.Add(new Transfer
            {
                Id = id,
                Amount = 0.0,
                Name = "Test",
                Reciever = "Test",
                FromBank = "Test",
                FromAccount = "Test",
                Description = "Test",
                DateWasMade = DateTime.UtcNow,
                TransferType = "Test",
                RecieverAccount = "Test",
                UserId = "1",
               

            });

            context.SaveChanges();


            var sut = new Delete.Handler(context, userAccessor.Object);
            var result = sut.Handle(new Delete.Command { Id = id }, CancellationToken.None).Result;
            Assert.True(result.IsSuccess);
            Assert.Null(await context.Transfers.FindAsync(id));
        }

        [Fact]
        public async void Should_Fail_2_delete_If_No_User()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");
            context.Transfers.Add(new Transfer
            {
                Id = id,
                Amount = 0.0,
                Name = "Test",
                Reciever = "Test",
                FromBank = "Test",
                FromAccount = "Test",
                Description = "Test",
                DateWasMade = DateTime.UtcNow,
                TransferType = "Test",
                RecieverAccount = "Test",
                UserId = "1",


            });

            context.SaveChanges();


            var sut = new Delete.Handler(context, userAccessor.Object);
            var result = sut.Handle(new Delete.Command { Id = id }, CancellationToken.None).Result;

            Assert.False(result.IsSuccess);
            Assert.NotNull(await context.Transfers.FindAsync(id));
        }
    }
}
