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
    public class Edit
    {
        public class Command : IRequest<Result<DeptReadDto>>
        {
            public Guid Id { get; set; }
            public DeptWriteDto Dept { get; set; }
        }


        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotEmpty();
                RuleFor(x => x.Dept).SetValidator(new DeptValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<DeptReadDto>>
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

            public async Task<Result<DeptReadDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var dept = await _dataContext.Depts.FindAsync(request.Id);
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());
                if (user == null || dept == null || dept.UserId != user.Id)
                       return Result<DeptReadDto>.Failure("Failed to edit dept");

                _mapper.Map(request.Dept, dept);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                          ? Result<DeptReadDto>.Failure("Failed to edit dept")
                          : Result<DeptReadDto>.Success(_mapper.Map<DeptReadDto>(dept));
            }
        }
    }
}
