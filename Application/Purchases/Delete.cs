using MediatR;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Purchases
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var purchase = await _dataContext.Purchases.FindAsync(request.Id);
                if (purchase != null)
                {
                _dataContext.Remove(purchase);
                    await _dataContext.SaveChangesAsync();
                } 

                return Unit.Value;
            }
        }
    }
}
