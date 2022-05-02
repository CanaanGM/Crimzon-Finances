using Application.Core;
using Application.Interfaces;

using AutoMapper;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Invoices
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid PurchaseId { get; set; }
            public Guid InvoiceId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PurchaseId).NotEmpty();
                RuleFor(c => c.InvoiceId).NotEmpty();
            }
        }


        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());
                var purchase = await _dataContext.Purchases.FirstOrDefaultAsync(p => p.Id == request.PurchaseId);
                var invoice = await _dataContext.Photos.FirstOrDefaultAsync(p => p.Id == request.InvoiceId);

                if (user == null || purchase == null || !user.Purchases.Contains(purchase) || !purchase.Invoice.Contains(invoice))
                    return Result<Unit>.Failure("Bitch!");

                _dataContext.Photos.Remove(invoice);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                      ? Result<Unit>.Failure("Failed to Delete invoice")
                      : Result<Unit>.Success(Unit.Value);


            }
        }
    }
}
