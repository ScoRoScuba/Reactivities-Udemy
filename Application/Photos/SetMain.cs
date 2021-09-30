using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Photos
{
    public class SetMain
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Command(string id)
            {
                Id = id;
            }
            public string Id { get; }
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
                var user = await _dataContext.Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                if (user == null) return null;

                var photo = user.Photos.FirstOrDefault(p => p.Id == request.Id);
                if (photo == null) return null;

                var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
                if (currentMain != null) currentMain.IsMain = false;

                photo.IsMain = true;

                var success = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                return success ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem setting Main");
            }
        }
    }
}
