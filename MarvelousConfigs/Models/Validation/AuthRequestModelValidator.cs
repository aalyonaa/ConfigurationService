using FluentValidation;
using Marvelous.Contracts.RequestModels;

namespace MarvelousConfigs.API.Models.Validation
{
    public class AuthRequestModelValidator : AbstractValidator<AuthRequestModel>
    {
        public AuthRequestModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }

    }
}
