using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Domain;

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
    public class List
    {
        public class Query : IRequest<Result<List<PhotoReadDto>>> 
        {
            public Guid PurchaseId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<PhotoReadDto>>>
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
            public async Task<Result<List<PhotoReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x=>x.Id == _userAccessor.GetUserId());
                if (user == null) return Result<List<PhotoReadDto>>.Failure("Bitch!");

                var purchases = await _dataContext.Purchases
                    .Where(x=>x.UserId == user.Id)
                    .ProjectTo<PurchaseReadDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();


                List<PhotoReadDto> photos = new();
                foreach (var purchase in purchases)
                    if (purchase.Id == request.PurchaseId)
                        foreach (var inv in purchase.Invoice)
                            photos.Add(inv);

                return Result<List<PhotoReadDto>>.Success(_mapper.Map <List<PhotoReadDto>>(photos));
            }
        }
    }
}
