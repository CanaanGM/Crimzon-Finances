using Application.Core;
using Application.DTOs;
using Application.Interfaces;

using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Persistence;

namespace Application.Transfers
{
    public class Details
    {
        public class Query : IRequest<Result<TransferReadDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<TransferReadDto>>
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

            public async Task<Result<TransferReadDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var transfer = await _dataContext.Transfers.FindAsync(request.Id);
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userAccessor.GetUserId());

                if (user == null || transfer == null || transfer.UserId != user.Id) 
                    return Result<TransferReadDto>.Failure("failed to fetch transfer . . .");
                return Result<TransferReadDto>.Success(_mapper.Map<TransferReadDto>(transfer));
            }
        }
    }
}