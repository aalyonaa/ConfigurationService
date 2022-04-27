using FluentValidation;

namespace MarvelousConfigs.API.Models.Validation
{
    public class ConfigInputModelValidator : AbstractValidator<ConfigInputModel>
    {
        public ConfigInputModelValidator()
        {
            RuleFor(x => x.Value).NotEmpty();
        }
    }
}
