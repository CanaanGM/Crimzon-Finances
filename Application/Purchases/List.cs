using Application.Core;
using Application.DTOs;

using AutoMapper;

using Domain;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            public Handler(DataContext context, IMapper mapper)// this context can be interchanged for another context
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<List<PurchaseReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return  Result<List<PurchaseReadDto>>.Success(_mapper.Map<List<PurchaseReadDto>>( await _context.Purchases.ToListAsync() ));
            }
        }
    }
}
