using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Profiles
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Command(Profile profile)
            {
                Profile = profile;
            }

            public Guid UserId { get; set; }
            public Profile Profile { get; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Profile).SetValidator(new ProfileValidator());
            }
        }

        public class EditHandler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public EditHandler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());
                if (user == null) return null;

                user.DisplayName = request.Profile.DisplayName ?? user.DisplayName;
                user.Bio = request.Profile.Bio ?? user.Bio;

                _dataContext.Entry(user).State = EntityState.Modified;

                var result = await _dataContext.SaveChangesAsync(true, cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to Update Profile");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
