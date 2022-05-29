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
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
            public FolderWriteDto Folder { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotEmpty();
                RuleFor(c=> c.Folder).SetValidator(new FolderValidator());
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
                var folder = await _dataContext.Folders.FindAsync(request.Id);
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());
                if (user == null || folder == null || folder.UserId != user.Id)
                    return Result<Unit>.Failure("Failed to edit dept");

                _mapper.Map(request.Folder, folder);
                var res = await _dataContext.SaveChangesAsync() > 0;
                return !res
                          ? Result<Unit>.Failure("Failed to edit folder")
                          : Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
