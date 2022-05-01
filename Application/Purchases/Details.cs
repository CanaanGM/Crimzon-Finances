using Application.Core;
using Application.DTOs;

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
    public class Details
    {
        public class Query : IRequest<Result<PurchaseReadDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PurchaseReadDto>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }
            public async Task<Result<PurchaseReadDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<PurchaseReadDto>.Success(_mapper.Map< PurchaseReadDto>( await _dataContext.Purchases.FindAsync(request.Id) ));
            }
        }
    }
}
