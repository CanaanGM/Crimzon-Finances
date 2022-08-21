using Application.Core;
using Application.DTOs;
using Application.Errors;
using Application.Interfaces;

using AutoMapper;
using AutoMapper.QueryableExtensions;

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
    public class Details
    {
        public class Query : IRequest<Result<DeptReadDto>>
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

        public class Handler : IRequestHandler<Query, Result<DeptReadDto>>
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
            public async Task<Result<DeptReadDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(
                    x => x.Id == _userAccessor.GetUserId());

                var dept = await _dataContext.Depts.FindAsync(request.Id);

                if (dept == null)
                    throw new RestException(System.Net.HttpStatusCode.NotFound, new {Dept = "Not Found"});

                var dept2return = _mapper.Map<DeptReadDto>(dept);
                    //await _dataContext.Depts
                    //.ProjectTo<DeptReadDto>(_mapper.ConfigurationProvider)
                    //.FirstOrDefaultAsync(x => x.Id == request.Id);

                return dept.UserId == user.Id
                    ? Result<DeptReadDto>.Success(dept2return)
                    : Result<DeptReadDto>.Failure("Biatch");
            }
        }
    }
}
