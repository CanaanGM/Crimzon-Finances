using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using Domain;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payments
{
    public class Details
    {
        public class Query : IRequest<Result<PaymentReadDto>>
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


        public class Handler : IRequestHandler<Query, Result<PaymentReadDto>>
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
            public async Task<Result<PaymentReadDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(
                    x => x.Id == _userAccessor.GetUserId());
                var payment = await _dataContext.Payments.FindAsync(request.Id);
                var dept = await _dataContext.Depts.FindAsync(payment?.DeptId);

                if (user == null || payment == null || !user.Depts.Contains(dept) && !dept.Payments.Contains(payment))
                    return Result<PaymentReadDto>.Failure("Bitch!");


                var payment2return = await _dataContext.Payments
                      .ProjectTo<PaymentReadDto>(_mapper.ConfigurationProvider)
                      .FirstOrDefaultAsync(x => x.Id == request.Id);


                return payment.DeptId == dept.Id && payment.UserId == user.Id
                        ? Result<PaymentReadDto>.Success(payment2return)
                        : Result<PaymentReadDto>.Failure("Biatch");

            }
        }
    }
}
