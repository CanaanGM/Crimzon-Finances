using Application.Depts;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Depts
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

            context.Depts.Add(new Dept
            {
                Id = id,
                Name = "Example A",
                Amount = 4550,
                AmountRemaining = 2880,
                DateDeptWasMade = DateTime.UtcNow,
                DatePaidOff = DateTime.UtcNow,
                PaidOff = false,
                Deptor = "User1",
                UserId = "1"
            });

            context.SaveChanges();


            var sut = new Delete.Handler(context, userAccessor.Object);
            var result = sut.Handle(new Delete.Command { Id = id }, CancellationToken.None).Result;
            Assert.True(result.IsSuccess);
            Assert.Null(await context.Depts.FindAsync(id));
        }

        [Fact]
        public async void Should_Fail_2_delete_If_No_User()
        {
            var context = GetDbContext();
            Guid id = new Guid("69f31c5d-5872-4d86-841b-9bca0a2bc79e");

            context.Depts.Add(new Dept
            {
                Id = id,
                Name = "Example A",
                Amount = 4550,
                AmountRemaining = 2880,
                DateDeptWasMade = DateTime.UtcNow,
                DatePaidOff = DateTime.UtcNow,
                PaidOff = false,
                Deptor = "User1",
                UserId = "1"
            });

            context.SaveChanges();


            var sut = new Delete.Handler(context, userAccessor.Object);
            var result = sut.Handle(new Delete.Command { Id = id }, CancellationToken.None).Result;

            Assert.False(result.IsSuccess);
            Assert.NotNull(await context.Depts.FindAsync(id));
        }
    }
}
