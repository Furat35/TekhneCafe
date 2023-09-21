using FluentValidation;
using TekhneCafe.Core.DTOs.Category;

namespace TekhneCafe.Business.ValidationRules.FluentValidations.Category
{
    public class CategoryAddDtoValidator : AbstractValidator<CategoryAddDto>
    {
        public CategoryAddDtoValidator()
        {
            RuleFor(_ => _.Name)
               .NotNull()
               .NotEmpty()
               .WithMessage("Category adı boş olamaz!")
               .MinimumLength(2)
               .WithMessage("Category adı en az 2 karakter olmalı !")
               .MaximumLength(50)
               .WithMessage("Category adı en fazla 50 karakter olabilir !");
        }
    }
}
