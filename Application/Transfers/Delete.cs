using Application.Core;

using MediatR;

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

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var transfer = await _dataContext.Transfers.FindAsync(request.Id);
                if (transfer == null) return null;

                _dataContext.Remove(transfer);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                    ? Result<Unit>.Failure("Failed to Delete Transfer")
                    : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}