﻿using Application.Core;

using Domain;

using FluentValidation;

using MediatR;

using Persistence;

namespace Application.Purchases
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Purchase Purchase { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() =>
                RuleFor(x => x.Purchase).SetValidator(new PurchaseValidator());
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
                _dataContext.Purchases.Add(request.Purchase);
                var result = await _dataContext.SaveChangesAsync() > 0;

               return !result ? Result<Unit>.Failure("Failed to create Purchase") : Result<Unit>.Success(Unit.Value);

                 
            }
        }
    }
}