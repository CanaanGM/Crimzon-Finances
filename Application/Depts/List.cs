using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Depts
{
    public class List
    {
        public class Query : IRequest<Result<CustomPagedList<DeptReadDto>>>
        {
            public PagedParams Params { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<CustomPagedList<DeptReadDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _context = context;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }
            public async Task<Result<CustomPagedList<DeptReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x =>
                            x.Id == _userAccessor.GetUserId());
                if (user == null) return Result<CustomPagedList<DeptReadDto>>.Failure("bitch");

                var query = _context.Depts
                    .Where(x => x.UserId == user.Id)
                    .ProjectTo<DeptReadDto>(_mapper.ConfigurationProvider)
                    .AsQueryable();




                return query == null
                    ? Result<CustomPagedList<DeptReadDto>>.Failure("Failed to get purchases")
                    : Result<CustomPagedList<DeptReadDto>>.Success(
                        await CustomPagedList<DeptReadDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize));
            }
        }
    }
}
