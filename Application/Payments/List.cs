using Application.Core;
using Application.DTOs;
using AutoMapper;
using Application.Interfaces;

using FluentValidation;

using MediatR;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Payments
{
    public class List
    {

        public class Query : IRequest<Result<List<PaymentReadDto>>>
        {
            public Guid Id { get; set; }
        }


        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }


        public class Handler : IRequestHandler<Query, Result<List<PaymentReadDto>>>
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

            public async Task<Result<List<PaymentReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());
                if (user == null) return Result<List<PaymentReadDto>>.Failure("Bitch!");

                var payment = await _dataContext.Payments.FindAsync(request.Id);
                if (payment == null) return Result<List<PaymentReadDto>>.Failure("Biatch!");

                var payments = await _dataContext.Depts
                    .Select(x => x.Payments)
                    .Where(x => x.Contains(payment))
                    .ToListAsync()
                    ;


                return Result<List<PaymentReadDto>>.Success(_mapper.Map<List<PaymentReadDto>>(payments));

            }
        }
    }
}
