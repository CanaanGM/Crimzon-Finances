using Application.Core;

using Domain;

using MediatR;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transfers
{
    public class Details
    {
        public class Query : IRequest<Result<Transfer>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<Transfer>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task<Result<Transfer>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<Transfer>.Success( await _dataContext.Transfers.FindAsync(request.Id) );
            }
        }
    }
}
