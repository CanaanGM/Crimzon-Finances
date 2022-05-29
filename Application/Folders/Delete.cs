using Application.Core;
using Application.Interfaces;

using Domain;

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
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
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
                var folder = await _dataContext.Folders.FindAsync(request.Id);
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());

                if (folder == null || folder.UserId != user?.Id) return Result<Unit>.Failure("Error Deleting . . .");
                _dataContext.Remove(folder);
                var res = await _dataContext.SaveChangesAsync() > 0;

                return !res
                    ? Result<Unit>.Failure("Failed to Delete folder")
                    : Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
