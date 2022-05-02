using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;

using Domain;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Application.Invoices
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid PurchaseId { get; set; }
            public PhotoWriteDto Invoice { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator() => RuleFor(x => x.Invoice).SetValidator(new InvoiceValidator());
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
                // get the current user
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());
                // get the purchase from id passed in via request
                var purchase = await _dataContext.Purchases.FirstOrDefaultAsync(x => x.Id == request.PurchaseId);
                if (user == null || purchase == null || !user.Purchases.Contains(purchase)) return Result<Unit>.Failure("Bitch!");

                // create (Decode) the photo
                List<Photo> photos = new();
                if (request.Invoice != null)
                    photos.AddRange(await GenerateInvoicesFromRequest(request.Invoice, purchase));
                // if it doesn't exisits in the purchase.Invoices, create and add it

                purchase.Invoice.AddRange(photos);
                var result = await _dataContext.SaveChangesAsync() > 0;

                return !result ? Result<Unit>.Failure("Failed to create Invoice") : Result<Unit>.Success(Unit.Value);

            }

            private async Task<List<Photo>> GenerateInvoicesFromRequest(PhotoWriteDto invoice, Purchase purchase)
            {
                List<Photo> photosList = new List<Photo>();
                foreach (var file in invoice.Files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                        var newInvoice = new Photo()
                        {
                            Name = file.FileName,
                            Description = $"{purchase.Name} Invoice",
                            FileExtension = file.ContentType,
                            Size = memoryStream.Length,
                            Bytes = memoryStream.ToArray(),
                            Purchase = purchase,
                            PurchaseId = purchase.Id,
                        };

                        if (!purchase.Invoice.Contains(newInvoice) || purchase.Invoice.Count == 0)
                        {
                            _dataContext.Photos.Add(newInvoice);

                            photosList.Add(newInvoice);
                        }
                    };
                }

                return photosList;
            }
        }
    }
}