using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Application.Purchases
{
    public class List
    {
        public class Query : IRequest<Result<List<PurchaseReadDto>>> // later to be a paginated list
        { }

        public class Handler : IRequestHandler<Query, Result<List<PurchaseReadDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)// this context can be interchanged for another context
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<List<PurchaseReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x =>
                            x.Id == _userAccessor.GetUserId());
                if (user == null ) return Result<List<PurchaseReadDto>>.Failure("bitch");
               
                var purchases = await _context.Purchases
                    .Where(x=>x.UserId == user.Id)
                    .ProjectTo<PurchaseReadDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return purchases == null
                    ? Result<List<PurchaseReadDto>>.Failure("Failed to get purchases")
                    : Result<List<PurchaseReadDto>>.Success(purchases);
            }
        }
    }
}