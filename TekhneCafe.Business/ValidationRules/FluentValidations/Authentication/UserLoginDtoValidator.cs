using FluentValidation;
using TekhneCafe.Core.DTOs.Authentication;

namespace TekhneCafe.Business.ValidationRules.FluentValidations.Authentication
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(_ => _.Username)
                .NotNull()
                .WithMessage("Kullanıcı adı boş olamaz!")
                .MinimumLength(5)
                .WithMessage("Kullanıcı adı en az 5 karakter içermelidir!")
                .MaximumLength(100)
                .WithMessage("Kullanıcı adı en az 5 en fazla 100 karakter içerebilir!");

            RuleFor(_ => _.Password)
                .NotNull()
                .WithMessage("Şifre boş olamaz")
                .MinimumLength(3)
                .WithMessage("Şifre en az 2 karakter içermelidir!");
        }
    }
}
