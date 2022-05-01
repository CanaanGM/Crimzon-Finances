﻿using Application.Core;

using MediatR;

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

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var purchase = await _dataContext.Purchases.FindAsync(request.Id);
                if (purchase == null) return null;

                _dataContext.Remove(purchase);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                    ? Result<Unit>.Failure("Failed to Delete purchase")
                    : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}