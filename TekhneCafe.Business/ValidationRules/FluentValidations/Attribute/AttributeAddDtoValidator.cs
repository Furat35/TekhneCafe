using FluentValidation;
using TekhneCafe.Core.DTOs.Attribute;

namespace TekhneCafe.Business.ValidationRules.FluentValidations.Attribute
{
    public class AttributeAddDtoValidator : AbstractValidator<AttributeAddDto>
    {
        public AttributeAddDtoValidator()
        {
            RuleFor(_ => _.Name)
               .NotNull()
               .NotEmpty()
               .WithMessage("Attribute adı boş olamaz!")
               .MinimumLength(2)
               .WithMessage("Attribute adı en az 2 karakter olmalı !")
               .MaximumLength(50)
               .WithMessage("Attribute adı en fazla 50 karakter olabilir !");
        }
    }
}
