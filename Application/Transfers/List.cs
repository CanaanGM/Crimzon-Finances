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

namespace Application.Transfers
{
    public class List
    {
        public class Query : IRequest<Result<List<TransferReadDto>>> // later to be a paginated list
        { }

        public class Handler : IRequestHandler<Query, Result<List<TransferReadDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)// this context can be interchanged for another context
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<Result<List<TransferReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<TransferReadDto>>.Success(_mapper.Map< List<TransferReadDto>>( await _context.Transfers.ToListAsync()));
            }
        }
    }
}
