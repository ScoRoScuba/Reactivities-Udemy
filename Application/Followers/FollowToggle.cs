using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Followers
{
    public class FollowToggle 
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Command(string userName)
            {
                TargetUserName = userName;
            }

            public string TargetUserName { get; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor )
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var observer =
                    await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                var target = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == request.TargetUserName);

                if (target == null) return null;

                var following = await _dataContext.UserFollowings.FindAsync(observer.Id, target.Id);
                if (following == null)
                {
                    following = new UserFollowing()
                    {
                        Observer = observer,
                        Target = target
                    };

                    _dataContext.UserFollowings.Add(following);
                }
                else
                {
                    _dataContext.UserFollowings.Remove(following);
                }

                var success = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                return success ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to save following");
            }
        }
    }
}
