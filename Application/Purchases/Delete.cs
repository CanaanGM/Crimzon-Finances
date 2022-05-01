using Application.Core;
using Application.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Application.Purchases
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var purchase = await _dataContext.Purchases.FindAsync(request.Id);
                var user = await _dataContext.Users.FirstOrDefaultAsync(x=>x.Id == _userAccessor.GetUserId());

                if (purchase == null || purchase.UserId != user?.Id) return Result<Unit>.Failure("Error Deleting. . .");

                _dataContext.Remove(purchase);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                    ? Result<Unit>.Failure("Failed to Delete purchase")
                    : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}