using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<PagedList<ActivityDto>>>
        {
            public Query() { }

            public Query(ActivityParams param)
            {
                PagingParams = param;
            }
            public ActivityParams PagingParams { get; }
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<ActivityDto>>>
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

            public async Task<Result<PagedList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _dataContext.Activities
                    .Where(d=>d.Date >= request.PagingParams.StartDate)
                    .OrderBy(d => d.Date)
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                        new { currentUsername = _userAccessor.GetUsername() })
                    .AsQueryable();

                if (request.PagingParams.IsGoing && !request.PagingParams.IsHost)
                {
                    query = query.Where(x => x.Attendees.Any(a => a.UserName == _userAccessor.GetUsername()));
                }

                if (request.PagingParams.IsGoing && !request.PagingParams.IsGoing)
                {
                    query.Where(x => x.HostUserName == _userAccessor.GetUsername());
                }

                var pagedList = await PagedList<ActivityDto>.CreateAsync(query,
                    request.PagingParams.PageNumber,
                    request.PagingParams.PageSize);

                return Result<PagedList<ActivityDto>>.Success(pagedList);
            }
        }

    }
}
