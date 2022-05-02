using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;

using Domain;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

using System.IO;

namespace Application.Purchases
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public PurchaseUpdateDto Purchase { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotEmpty();
                RuleFor(x => x.Purchase).SetValidator(new PurchaseUpdateValidator());
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

                if (request.Purchase.Files != null)
                    await UpdatePhotos(request.Purchase, purchase);

                _mapper.Map(request.Purchase, purchase);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                          ? Result<Unit>.Failure("Failed to edit purchase")
                          : Result<Unit>.Success(Unit.Value);
            }

            private async Task UpdatePhotos(PurchaseUpdateDto purchasedto, Purchase purchase)
            {   //! replaces the old pics with the new ones
                purchase.Invoice.Clear();

                foreach (var file in purchasedto.Files)
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

                        _dataContext.Photos.Add(newInvoice);

                        purchase.Invoice.Add(newInvoice);
                    }
                }
            }
        }




    }
}