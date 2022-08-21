using Application.Depts;

using Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApplicationTests.Depts
{
    public class ListTest : TestBase
    {
        [Fact]
        public void Should_Return_List()
        {
            var context = GetDbContext();

            context.Users.AddAsync(new Domain.AppUser { Id = "1", Email = "test@example.com", UserName = "test", DisplayName = "test" });
            context.SaveChangesAsync();


            context.Depts.Add(new Dept {
                    Name = "Example A",
                    Amount = 4550,
                    AmountRemaining = 2880,
                    DateDeptWasMade = DateTime.UtcNow,
                    DatePaidOff = DateTime.UtcNow,
                    PaidOff = false,
                    Deptor = "User1",
                    UserId = "1"
                 });
            context.Depts.Add(new Dept
            {
                Name = "Example B",
                Amount = 90,
                AmountRemaining = 0.0,
                DateDeptWasMade = DateTime.UtcNow,
                DatePaidOff = DateTime.UtcNow,
                PaidOff = true,
                Deptor = "User1",
                UserId = "1"
            });

            context.Depts.Add(new Dept
            {
                Name = "Example B",
                Amount = 666,
                AmountRemaining = 0.0,
                DateDeptWasMade = DateTime.UtcNow,
                DatePaidOff = DateTime.UtcNow,
                PaidOff = true,
                Deptor = "Dark Schinder",
                UserId = "2"
            });

            context.Depts.Add(new Dept
            {
                Name = "Example C",
                Amount = 92103,
                AmountRemaining = 91000,
                DateDeptWasMade = DateTime.UtcNow,
                DatePaidOff = DateTime.UtcNow,
                PaidOff = false,
                Deptor = "redGrave Pizzaria",
                UserId = "3"
            });

            context.SaveChanges();


            var sut = new List.Handler(context, _mapper, userAccessor.Object);
            var result = sut.Handle(new List.Query { Params=new Application.Core.PagedParams { } }, CancellationToken.None).Result;

            Assert.NotNull(result);
            Assert.Equal(2, result.Value.Count);
            Assert.Equal("Example A", result.Value[0].Name);
        }

        //TODO: mopre testing 
    }
}
