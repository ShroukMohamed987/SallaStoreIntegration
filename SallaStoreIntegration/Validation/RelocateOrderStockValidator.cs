using FluentValidation;
using SallaStoreIntegration.Dtos.Orders;

namespace SallaStoreIntegration.Validation
{
    public class RelocateOrderStockValidator : AbstractValidator<RelocateOrderStockRequestDto>
    {
        public RelocateOrderStockValidator()
        {
            RuleFor(x => x.Source)
                .GreaterThan(0).WithMessage("source (branch id) is required.");

            RuleFor(x => x.Destination)
                .GreaterThan(0).WithMessage("destination (branch id) is required.");

            RuleFor(x => x.Destination)
                .NotEqual(x => x.Source).WithMessage("destination must be different from source.");

            RuleForEach(x => x.Items).SetValidator(new RelocateOrderStockItemValidator());
        }
    }

    public class RelocateOrderStockItemValidator : AbstractValidator<RelocateOrderStockItemDto>
    {
        public RelocateOrderStockItemValidator()
        {
            RuleFor(i => i.Id).GreaterThan(0).WithMessage("items[].id is required.");
            RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("items[].quantity must be greater than 0.");
        }
    }
}
