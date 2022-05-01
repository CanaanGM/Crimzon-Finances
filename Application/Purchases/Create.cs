using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;

using Domain;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Application.Purchases
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public PurchaseWriteDto Purchase { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() =>
                RuleFor(x => x.Purchase).SetValidator(new PurchaseValidator());
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IUserAccessor userAccessor, IMapper mapper)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x =>
                    x.UserName == _userAccessor.GetUsername()
                  );

                if (user == null) return Result<Unit>.Failure("Failed to create purchase . . .");

                var purchase = _mapper.Map<Purchase>(request.Purchase);
                purchase.User = user;
                purchase.UserId = user.Id;
                user.Purchases.Add(purchase);

                _dataContext.Purchases.Add(purchase);
                var result = await _dataContext.SaveChangesAsync() > 0;

                return !result ? Result<Unit>.Failure("Failed to create Purchase") : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}