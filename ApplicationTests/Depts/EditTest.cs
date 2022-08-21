using Application.Depts;
using Application.DTOs;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Depts
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

            var deptInfo = new DeptWriteDto 
            {
                Name = "Example A",
                Amount = 4550,
                AmountRemaining = 1550,
                DateDeptWasMade = DateTime.UtcNow,
                DatePaidOff = DateTime.UtcNow,
                PaidOff = false,
                Deptor = "User1",
            };
            
            var sut = new Edit.Handler(context, _mapper, userAccessor.Object);

            var result = sut.Handle(new Edit.Command { Id = id, Dept = deptInfo }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            Assert.Equal("Example A", result.Value.Name);

        }
    }
}
