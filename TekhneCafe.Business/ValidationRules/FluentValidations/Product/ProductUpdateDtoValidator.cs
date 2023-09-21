using FluentValidation;
using TekhneCafe.Core.DTOs.Product;

namespace TekhneCafe.Business.ValidationRules.FluentValidations.Product
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(_ => _.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ürün adı boş olamaz!")
                .MinimumLength(2)
                .WithMessage("Ürün adı en az 2 karakter olmalı !")
                .MaximumLength(50)
                .WithMessage("Ürün adı en fazla 50 karakter olabilir !");

            RuleFor(_ => _.Description)
                .MaximumLength(150)
                .WithMessage("Açıklama en fazla 50 karakter olabilir !");

            RuleFor(_ => _.Price)
                .NotNull()
                .WithMessage("Fiyat boş olamaz !")
                .NotEmpty()
                .WithMessage("Fiyat boş olamaz !")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Fiyat 0 dan küçük olamaz !")
                .LessThan(1000)
                .WithMessage("Fiyat 1000 TL den fazla olamaz !");

            RuleFor(_ => _.CategoryId)
               .NotEmpty()
               .WithMessage("CategoryId boş olamaz!")
               .NotNull()
               .WithMessage("CategoryId boş olamaz!");
        }
    }
}
