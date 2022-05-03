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
        public class Query : IRequest<Result<CustomPagedList<PurchaseReadDto>>> // later to be a paginated list
        { public PurchaseParams PagedParams { get; set; } }

        public class Handler : IRequestHandler<Query, Result<CustomPagedList<PurchaseReadDto>>>
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

            public async Task<Result<CustomPagedList<PurchaseReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x =>
                            x.Id == _userAccessor.GetUserId());
                if (user == null ) return Result<CustomPagedList<PurchaseReadDto>>.Failure("bitch");
               
                var query =  _context.Purchases
                    .Where(x=>x.UserId == user.Id)
                    .Where(x=>x.PurchaseDate >= request.PagedParams.StartDate)
                    .OrderBy(c=>c.PurchaseDate)
                    .ProjectTo<PurchaseReadDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();

                if (request.PagedParams.StartDate != DateTime.UtcNow
                    && request.PagedParams.EndDate != new DateTime())
                    query = query.Where(x => x.PurchaseDate.Month >= request.PagedParams.StartDate.Month &&
                                x.PurchaseDate.Month <= request.PagedParams.EndDate.Month);
                else if (request.PagedParams.EndDate != new DateTime())
                    query = query.Where(x => x.PurchaseDate.Month <= request.PagedParams.StartDate.Month);

                else if (request.PagedParams.StartDate != DateTime.UtcNow)
                    query = query.Where(x => x.PurchaseDate.Month >= request.PagedParams.StartDate.Month);



                return query == null
                    ? Result<CustomPagedList<PurchaseReadDto>>.Failure("Failed to get purchases")
                    : Result<CustomPagedList<PurchaseReadDto>>.Success(
                        await CustomPagedList<PurchaseReadDto>.CreateAsync(query,
                    request.PagedParams.PageNumber, request.PagedParams.MaxPageSize));
            }
        }
    }
}