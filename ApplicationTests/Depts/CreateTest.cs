using Application.Depts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.Depts
{
    public class CreateTest : TestBase
    {
        [Fact]
        public void Should_Create()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();

            var dept = new Create.Command
            {
                Dept = new Application.DTOs.DeptWriteDto
                {
                      Name= "Dept A",
                      Amount= 450,
                      AmountRemaining= 880,
                      DateDeptWasMade= DateTime.UtcNow,
                      DatePaidOff= DateTime.UtcNow,
                      PaidOff= true,
                      Deptor= "user1"
                }
            };


            var sut = new Create.Handler(context, userAccessor.Object);
            var result = sut.Handle(dept, CancellationToken.None).Result;
            
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

        }

        [Fact]
        public void Should_Fail_With_No_User()
        {
            var context = GetDbContext();

            var dept = new Create.Command
            {
                Dept = new Application.DTOs.DeptWriteDto
                {
                    Name = "DeptA",
                    Amount = 450,
                    AmountRemaining = 280,
                    DateDeptWasMade = DateTime.UtcNow,
                    DatePaidOff = DateTime.UtcNow,
                    PaidOff = true,
                    Deptor = "user1"
                }
            };


            var sut = new Create.Handler(context, userAccessor.Object);
            var result = sut.Handle(dept, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
        }
    }
}
