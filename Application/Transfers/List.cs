using Application.Core;

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
        public class Query : IRequest<Result<List<Transfer>>> // later to be a paginated list
        { }

        public class Handler : IRequestHandler<Query, Result<List<Transfer>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)// this context can be interchanged for another context
            {
                _context = context;
            }
            public async Task<Result<List<Transfer>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<Transfer>>.Success(await _context.Transfers.ToListAsync());
            }
        }
    }
}
