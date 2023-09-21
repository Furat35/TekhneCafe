using FluentValidation;
using TekhneCafe.Core.DTOs.Payment;

namespace TekhneCafe.Business.ValidationRules.FluentValidations.Payment
{
    public class PaymentDtoValidator : AbstractValidator<PaymentDto>
    {
        public PaymentDtoValidator()
        {
            RuleFor(_ => _.Description)
                .MaximumLength(200)
                .WithMessage("Açıklama en fazla 200 karakter içerebilir!");

            RuleFor(_ => _.Amount)
                .GreaterThan(0)
                .WithMessage("Girilen miktar 0'dan büyük olmalıdır!");

            RuleFor(_ => _.UserId)
                .NotNull()
                .WithMessage("Geçersiz kullanıcı bilgisi!")
                .NotEmpty()
                .WithMessage("Geçersiz kullanıcı bilgisi!");
        }
    }
}
