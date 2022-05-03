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


namespace Application.Transfers
{
    public class List
    {
        public class Query : IRequest<Result<CustomPagedList<TransferReadDto>>> // later to be a paginated list
        {
            public TransferParams PagedParams { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CustomPagedList<TransferReadDto>>>
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
            public async Task<Result<CustomPagedList<TransferReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x =>
               x.Id == _userAccessor.GetUserId());
                if (user == null) return Result<CustomPagedList<TransferReadDto>>.Failure("bitch");

                var query =_context.Transfers
                     .Where(x => x.UserId == user.Id)
                     .Where(x=>x.DateWasMade >= request.PagedParams.StartDate)
                     .OrderBy(x => x.DateWasMade)
                     .ProjectTo<TransferReadDto>(_mapper.ConfigurationProvider)
                     .AsQueryable();
                    ;



                if (request.PagedParams.StartDate != DateTime.UtcNow 
                    && request.PagedParams.EndDate != new DateTime() )
                    query = query.Where(x => x.DateWasMade.Month >= request.PagedParams.StartDate.Month &&
                                x.DateWasMade.Month <= request.PagedParams.EndDate.Month);
                else if (request.PagedParams.EndDate != new DateTime() )
                    query = query.Where(x => x.DateWasMade.Month <= request.PagedParams.StartDate.Month);

                else if (request.PagedParams.StartDate != DateTime.UtcNow)
                    query =  query.Where(x => x.DateWasMade.Month >= request.PagedParams.StartDate.Month);
                


                return Result<CustomPagedList<TransferReadDto>>.Success(
                    await CustomPagedList<TransferReadDto>.CreateAsync(query,
                    request.PagedParams.PageNumber, request.PagedParams.MaxPageSize)
                    );
            }
        }
    }
}
