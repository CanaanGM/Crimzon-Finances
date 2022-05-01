﻿using Application.Core;

using AutoMapper;

using Domain;

using FluentValidation;

using MediatR;

using Persistence;

namespace Application.Purchases
{
    public class Edit
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
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var purchase = await _dataContext.Purchases.FindAsync(request.Purchase.Id);
                if (purchase == null) return null;

                _mapper.Map(request.Purchase, purchase);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                          ? Result<Unit>.Failure("Failed to edit purchase")
                          : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}