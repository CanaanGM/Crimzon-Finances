using Application.Core;
using Application.DTOs;
using Application.Interfaces;
using Application.Purchases;

using AutoMapper;

using Domain;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Application.Transfers
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public TransferWriteDto Transfer { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() =>
                RuleFor(x => x.Transfer).SetValidator(new TransferValidator());
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
                var transfer = _mapper.Map<Transfer>(request.Transfer);
                var user = await _dataContext.Users.FirstOrDefaultAsync(x=>x.Id == _userAccessor.GetUserId());
                if (user == null) return Result<Unit>.Failure("Failed to create transfer");
                transfer.User = user;
                transfer.User.Id = user.Id;

                user.Transfers.Add(transfer);

                _dataContext.Transfers.Add(transfer);

                var result = await _dataContext.SaveChangesAsync() > 0;

                return !result ? Result<Unit>.Failure("Failed to create Transfer") : Result<Unit>.Success(Unit.Value);


            }
        }
    }
}