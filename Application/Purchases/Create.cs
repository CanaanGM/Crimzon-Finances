using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;

using Domain;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

using System.IO.Compression;

namespace Application.Purchases
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public PurchaseWriteDto Purchase { get; set; }
            public PhotoWriteDto Photos { get; set; }
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

                  
                var purchase =  CreatePurchase(request, user);
                var photos = new List<Photo>();

                purchase.Invoice.AddRange(photos);
                user.Purchases.Add(purchase);

                _dataContext.Purchases.Add(purchase);

                if (request.Photos != null)
                  photos.AddRange( await  GeneratePhotosFromRequest(purchase, request.Photos) );
                
                
                var result = await _dataContext.SaveChangesAsync() > 0;

                return !result ? Result<Unit>.Failure("Failed to create Purchase") : Result<Unit>.Success(Unit.Value);
            }

            private async Task<List<Photo>> GeneratePhotosFromRequest(Purchase purchase, PhotoWriteDto photos)
            {

                List<Photo> photosList = new List<Photo>();
                foreach (var file in photos.Files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);

                            var newInvoice = new Photo()
                            {
                                Name = file.FileName,
                                Description = $"{purchase.Name} Invoice",
                                FileExtension= file.ContentType,
                                Size = memoryStream.Length,
                                Bytes = memoryStream.ToArray(),
                                Purchase = purchase,
                                PurchaseId = purchase.Id,
                            };

                        _dataContext.Photos.Add(newInvoice);

                        photosList.Add(newInvoice);
                    };

                }
                
                return photosList;

            }

            private  Purchase CreatePurchase(Command request, AppUser user)
            {

                return new Purchase {
                    Name = request.Purchase.Name,
                    Seller = request.Purchase.Seller,
                    PurchaseDate = request.Purchase.PurchaseDate,
                    Category = request.Purchase.Category,
                    Price = request.Purchase.Price,
                    PriceInDollar = request.Purchase.PriceInDollar,
                    Description = request.Purchase.Description,
                    PaymentMethod = request.Purchase.PaymentMethod,
                    Reccuring = request.Purchase.Reccuring,
                    User = user,
                    UserId = user.Id
                };
            }
        }
    }
}