using Application.Core;
using Application.Purchases;

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

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _dataContext.Transfers.Add(request.Transfer);
                var result = await _dataContext.SaveChangesAsync() > 0;

                return !result ? Result<Unit>.Failure("Failed to create Transfer") : Result<Unit>.Success(Unit.Value);


            }
        }
    }
}