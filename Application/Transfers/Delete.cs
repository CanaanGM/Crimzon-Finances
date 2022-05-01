using Application.Core;
using Application.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Application.Transfers
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
                var transfer = await _dataContext.Transfers.FindAsync(request.Id);
                var user = await _dataContext.Users.FirstOrDefaultAsync(x=>x.Id == _userAccessor.GetUserId());

                if (transfer == null || user == null || transfer.UserId != user.Id)
                    return Result<Unit>.Failure("Failed to delete. . .");

                _dataContext.Remove(transfer);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                    ? Result<Unit>.Failure("Failed to Delete Transfer")
                    : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}