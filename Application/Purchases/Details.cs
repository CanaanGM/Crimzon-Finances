﻿using Application.Core;
using Application.DTOs;
using Application.Errors;
using Application.Interfaces;

using AutoMapper;
using AutoMapper.QueryableExtensions;

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

                var purchase = await _dataContext.Purchases
                    .FindAsync(request.Id);
                if (purchase == null) throw new RestException(System.Net.HttpStatusCode.NotFound, new { Purchase = "Not Found" });
                if (user == null || purchase.UserId != user.Id) return Result<PurchaseReadDto>.Failure("bitch");




                var purchase2Return = await _dataContext.Purchases
                    .ProjectTo<PurchaseReadDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (purchase.UserId == user.Id)
                return Result<PurchaseReadDto>.Success(purchase2Return);
                return Result<PurchaseReadDto>.Failure("biatch");
            }
        }
    }
}
