using FluentValidation;

namespace Application.Profiles
{
    class ProfileValidator : AbstractValidator<Profile>
    {
        public ProfileValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }
}

