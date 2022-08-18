using Application.Core;
using Application.DTOs;
using Application.Errors;
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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Folders
{
    public class Details
    {
        public class Query : IRequest<Result<FolderReadDto>>
        {
            public Guid Id { get; set; }
        }

        public class QueryValidator: AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, Result<FolderReadDto>>
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
            public async Task<Result<FolderReadDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(
                    x => x.Id == _userAccessor.GetUserId());

                var folder = await _dataContext.Folders.FindAsync(request.Id);
                if (folder == null)
                    throw new RestException(HttpStatusCode.NotFound, new { Folder = "Not Found" });

                var folder2return = await _dataContext.Folders
                    .ProjectTo<FolderReadDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x=>x.Id == request.Id);

                return folder.UserId == user.Id
                    ? Result<FolderReadDto>.Success(folder2return)
                    : Result<FolderReadDto>.Failure("Biatch");
            }
        }
    }
}
