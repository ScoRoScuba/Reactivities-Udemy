using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Persistance;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Command(Guid id)
            {
                Id = id;
            }

            public Guid Id { get; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities.FindAsync(request.Id);

                if (activity == null) return null;

                _dataContext.Remove(activity);

                var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                if(!result) return Result<Unit>.Failure("Failed to delete Activity");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
