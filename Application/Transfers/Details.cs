using Application.Core;
using Application.DTOs;

using AutoMapper;

using MediatR;

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

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<TransferReadDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<TransferReadDto>.Success(_mapper.Map<TransferReadDto>(await _dataContext.Transfers.FindAsync(request.Id)));
            }
        }
    }
}