﻿using Domain;

using MediatR;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Purchases
{
    public class Create
    {
        public class Command : IRequest 
        {
            public Purchase Purchase { get; set; }
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
                _dataContext.Purchases.Add(request.Purchase);
               await _dataContext.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}