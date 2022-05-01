using Application.Core;
using Application.DTOs;
using Application.Interfaces;

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
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }
            public async Task<Result<PurchaseReadDto>> Handle(Query request, CancellationToken cancellationToken)
{
                var user = await _dataContext.Users.FirstOrDefaultAsync(x =>
                     x.Id == _userAccessor.GetUserId());

                var purchase = await _dataContext.Purchases.FindAsync(request.Id);
                if (user == null || purchase == null || purchase.UserId != user.Id) return Result<PurchaseReadDto>.Failure("bitch");

                if (purchase.UserId == user.Id)
                return Result<PurchaseReadDto>.Success(
                    _mapper.Map< PurchaseReadDto>(purchase)
                    );
                return Result<PurchaseReadDto>.Failure("biatch");
            }
        }
    }
}
