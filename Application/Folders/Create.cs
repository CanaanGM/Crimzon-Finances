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

namespace Application.Folders
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public FolderWriteDto Folder { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Folder).SetValidator(new FolderValidator());
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
                if (user == null) return Result<Unit>.Failure("Failes to create Folder . . .");


                var folder = new Folder {
                    Name = request.Folder.Name,
                    UserId = user.Id,
                    User = user
                };

                user.Folders.Add(folder);
                _dataContext.Folders.Add(folder);

                var result = await _dataContext.SaveChangesAsync() > 0;
                return !result
                    ? Result<Unit>.Failure("Failed to create folder")
                    : Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
