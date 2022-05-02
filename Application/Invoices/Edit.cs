using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;

using Domain;

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
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid PurchaseId { get; set; }
            public Guid InvoiceId { get; set; }
            public PhotoUpdateDto Invoice { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PurchaseId).NotEmpty();
                RuleFor(c => c.InvoiceId).NotEmpty();
                RuleFor(c => c.Invoice).SetValidator(new InvoiceUpdateValidator());
            }
        }


        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IMapper mapper,IUserAccessor userAccessor )
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x=>x.Id == _userAccessor.GetUserId());
                var purchase = await _dataContext.Purchases.FirstOrDefaultAsync(p => p.Id == request.PurchaseId);
                var invoice = await _dataContext.Photos.FirstOrDefaultAsync(p => p.Id == request.InvoiceId);

                if (user == null || purchase == null || (!user.Purchases.Contains(purchase) && !purchase.Invoice.Contains(invoice)))
                    return Result<Unit>.Failure("Bitch!");

                var invoiceFromRequest = await DecodeInvoices(request.Invoice, purchase);

                invoiceFromRequest.Id = request.InvoiceId;
                _mapper.Map(invoiceFromRequest, invoice);

                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                      ? Result<Unit>.Failure("Failed to edit invoice")
                      : Result<Unit>.Success(Unit.Value);

            }

            private async Task<Photo> DecodeInvoices(PhotoUpdateDto invoices, Domain.Purchase purchase)
            {
                    using (var memoryStream = new MemoryStream())
                    {
                        await invoices.File.CopyToAsync(memoryStream);
                        return new Photo()
                        {
                            Name = invoices.File.FileName,
                            Description = $"{purchase.Name}\n\t {purchase.Description} Invoice",
                            FileExtension = invoices.File.ContentType,
                            Size = memoryStream.Length,
                            Bytes = memoryStream.ToArray(),
                            Purchase = purchase,
                            PurchaseId = purchase.Id,
                        };
                    }
                
            }
        }
    }
}
