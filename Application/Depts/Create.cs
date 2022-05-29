using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;

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

namespace Application.Depts
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public DeptWriteDto Dept { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
               RuleFor(x => x.Dept).SetValidator(new DeptValidator()); 
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
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                if (user == null) return Result<Unit>.Failure("Failes to create Dept . . .");

                var dept = new Dept 
                    {
                        Name = request.Dept.Name,
                        Amount = request.Dept.Amount,
                        AmountRemaining = request.Dept.AmountRemaining,
                        DateMade = request.Dept.DateMade,
                        PaidOff = request.Dept.PaidOff,
                        Deptor = request.Dept.Deptor,
                        UserId = user.Id,
                        User = user
                    };


                user.Depts.Add(dept);
                _dataContext.Depts.Add(dept);

                var result = await _dataContext.SaveChangesAsync() > 0;
                return !result
                    ? Result<Unit>.Failure("Failed to create dept")
                    : Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
