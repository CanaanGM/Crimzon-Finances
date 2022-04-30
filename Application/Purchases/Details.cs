using Domain;

using MediatR;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Purchases
{
    public class Details
    {
        public class Query : IRequest<Purchase>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Purchase>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task<Purchase> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _dataContext.Purchases.FindAsync(request.Id);
            }
        }
    }
}
