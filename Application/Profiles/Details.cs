using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Profiles
{
    public class Details
    {
        public class Query : IRequest<Result<Profile>>
        {
            public Query(string username)
            {
                Username = username;
            }

            public string Username { get; }
            
        }

        public class Handler : IRequestHandler<Query, Result<Profile>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users
                    .ProjectTo<Profile>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(x => x.UserName == request.Username);

                if (user == null) return null;

                return Result<Profile>.Success(user);
            }
        }
    }
}
