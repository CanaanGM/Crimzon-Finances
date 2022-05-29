using Application.Core;
using Application.DTOs;
using Application.Interfaces;

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
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid DeptId { get; set; }
            public PaymentWriteDto Payment { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.DeptId).NotEmpty();
                RuleFor(c => c.Payment.Name).NotEmpty();
                RuleFor(c => c.Payment.Amount).NotEmpty();
                RuleFor(c => c.Payment.DateMade).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(
                    x => x.Id == _userAccessor.GetUserId());
                var dept = await _dataContext.Depts.FindAsync(request.DeptId);
                if (user == null || dept == null || !user.Depts.Contains(dept)) return Result<Unit>.Failure("Bitch!");

                var payment = new Domain.Payment
                {
                    Amount = request.Payment.Amount,
                    Name = request.Payment.Name,
                    DateMade = request.Payment.DateMade,
                    Dept = dept,
                    DeptId = dept.Id,
                    User = user,
                    UserId = user.Id
                };

                dept.Payments.Add(payment);
                user.Payments.Add(payment);


                var result = await _dataContext.SaveChangesAsync() > 0;

                return !result ? Result<Unit>.Failure("Failed to create Payment") : Result<Unit>.Success(Unit.Value);


            }
        }
    }
}
