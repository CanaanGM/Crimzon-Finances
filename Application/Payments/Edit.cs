using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;

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
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid DeptId { get; set; }
            public Guid PaymentId { get; set; }
            public PaymentWriteDto Pyament { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(a => a.DeptId).NotEmpty();
                RuleFor(e => e.PaymentId).NotEmpty();
            }
        }


        public class Handler : IRequestHandler<Command, Result<Unit>>
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
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(
                    x => x.Id == _userAccessor.GetUserId());
                var dept = await _dataContext.Depts.FindAsync(request.DeptId);
                var payment = await _dataContext.Payments.FindAsync(request.PaymentId);

                if (user == null || payment == null || !user.Depts.Contains(dept) && !dept.Payments.Contains(payment))
                    return Result<Unit>.Failure("Bitch!");

                _mapper.Map(request.Pyament, payment);

                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                      ? Result<Unit>.Failure("Failed to Edit payment")
                      : Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
