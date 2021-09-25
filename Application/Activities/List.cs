using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Activities
{
    public class List
    {
        public class Query : IRequest<Result<List<Activity>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<Activity>>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _dataContext.Activities.ToListAsync(cancellationToken: cancellationToken);
                return Result<List<Activity>>.Success(result);
            }
        }

    }
}
