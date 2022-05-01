using Application.Core;

using AutoMapper;

using Domain;

using FluentValidation;

using MediatR;

using Persistence;

namespace Application.Transfers
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Transfer Transfer { get; set; }
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
                var transfer = await _dataContext.Transfers.FindAsync(request.Transfer.Id);
                if (transfer == null) return null;

                _mapper.Map(request.Transfer, transfer);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                          ? Result<Unit>.Failure("Failed to edit transfer")
                          : Result<Unit>.Success(Unit.Value);
            }
        }
    }
}