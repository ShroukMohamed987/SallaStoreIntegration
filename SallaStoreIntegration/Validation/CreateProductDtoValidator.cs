using FluentValidation;
using SallaStoreIntegration.Dtos.Product;

namespace SallaStoreIntegration.Validation
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value.");

            RuleFor(x => x.product_type)
                .IsInEnum().WithMessage("Invalid product type value.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be 0 or greater.");

            RuleFor(x => x.Categories)
                .NotEmpty().WithMessage("At least one category is required.")
                .Must(c => c.All(id => id > 0)).WithMessage("All category IDs must be greater than 0.");

            RuleFor(x => x.sale_price)
                .GreaterThanOrEqualTo(0).WithMessage("Sale price must be 0 or greater.")
                .LessThanOrEqualTo(x => x.Price).WithMessage("Sale price must not exceed the original price.");

            RuleFor(x => x.cost_price)
                .GreaterThanOrEqualTo(0).WithMessage("Cost price must be 0 or greater.");

            RuleFor(x => x.SKU)
                .NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(100).WithMessage("SKU must not exceed 100 characters.");

            RuleFor(x => x.BrandId)
                .GreaterThan(0).WithMessage("Brand ID must be greater than 0.")
                .When(x => x.BrandId.HasValue);

            RuleForEach(x => x.Images)
                .SetValidator(new ImageDtoValidator())
                .When(x => x.Images != null && x.Images.Any());
        }
    }
    public class ImageDtoValidator : AbstractValidator<imageDto>
    {
        public ImageDtoValidator()
        {
            RuleFor(x => x.Original)
                .NotEmpty().WithMessage("Original image URL is required.")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("Original must be a valid URL.");

            RuleFor(x => x.Thumbnail)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("Thumbnail must be a valid URL.")
                .When(x => !string.IsNullOrEmpty(x.Thumbnail));

            RuleFor(x => x.Sort)
                .GreaterThanOrEqualTo(0).WithMessage("Sort must be 0 or greater.")
                .When(x => x.Sort.HasValue);
        }
    }
}
