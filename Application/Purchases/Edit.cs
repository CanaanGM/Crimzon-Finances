using AutoMapper;

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
    public class Edit
    {
        public class Command : IRequest {
            public Purchase Purchase { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var purchase = await _dataContext.Purchases.FindAsync(request.Purchase.Id);

                _mapper.Map(request.Purchase, purchase);    
                await _dataContext.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}
