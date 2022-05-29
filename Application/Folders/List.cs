using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Folders
{
    public class List
    {
        public class Query : IRequest<Result<List<FolderReadDto>>> { }

        public class Handler : IRequestHandler<Query, Result<List<FolderReadDto>>>
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

            public async Task<Result<List<FolderReadDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());
                if (user == null) return Result<List<FolderReadDto>>.Failure("Bitch!");


                var folders = await _dataContext.Folders
                   .Where(x => x.UserId == user.Id)
                   .ProjectTo<FolderReadDto>(_mapper.ConfigurationProvider)
                   .ToListAsync();

                return Result<List<FolderReadDto>>.Success(folders);
            }
        }
    }
}
