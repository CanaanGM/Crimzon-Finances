﻿using Application.Core;

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
        public class Query : IRequest<Result<List<Purchase>>> // later to be a paginated list
        { }

        public class Handler : IRequestHandler<Query, Result<List<Purchase>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)// this context can be interchanged for another context
            {
                _context = context;
            }
            public async Task<Result<List<Purchase>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<Purchase>>.Success(await _context.Purchases.ToListAsync());
            }
        }
    }
}
