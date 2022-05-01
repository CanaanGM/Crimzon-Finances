using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Application.Purchases
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public PurchaseWriteDto Purchase { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotEmpty();
                RuleFor(x => x.Purchase).SetValidator(new PurchaseValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var purchase = await _dataContext.Purchases.FindAsync(request.Id);
                var user = await _dataContext.Users.FirstOrDefaultAsync(x =>
            x.Id == _userAccessor.GetUserId());

                if (user == null || purchase == null || purchase.UserId != user.Id) 
                        return Result<Unit>.Failure("Failed to edit purchase");

                _mapper.Map(request.Purchase, purchase);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                          ? Result<Unit>.Failure("Failed to edit purchase")
                          : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}