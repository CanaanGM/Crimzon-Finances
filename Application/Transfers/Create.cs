using Application.Core;
using Application.DTOs;
using Application.Purchases;

using AutoMapper;

using Domain;

using FluentValidation;

using MediatR;

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

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _dataContext.Transfers.Add(_mapper.Map<Transfer>( request.Transfer));
                var result = await _dataContext.SaveChangesAsync() > 0;

                return !result ? Result<Unit>.Failure("Failed to create Transfer") : Result<Unit>.Success(Unit.Value);


            }
        }
    }
}