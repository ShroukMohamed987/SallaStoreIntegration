using FluentValidation;
using SallaStoreIntegration.Dtos.Orders;
using SallaStoreIntegration.Enums.Orders;

namespace SallaStoreIntegration.Validation
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequestDto>
    {
        public CreateOrderValidator()
        {
            // Customer — id OR (mobile + name) required
            When(x => x.Customer != null, () =>
            {
                RuleFor(x => x.Customer!)
                    .Must(c => c.Id.HasValue || (!string.IsNullOrWhiteSpace(c.Mobile) && !string.IsNullOrWhiteSpace(c.Name)))
                    .WithMessage("customer requires either 'id' OR both 'mobile' and 'name'.");
            });

            // Delivery & shipping
            When(x => x.DeliveryMethod == DeliveryMethodEnum.Shipping, () =>
            {
                RuleFor(x => x.CourierId)
                    .NotEmpty().WithMessage("courier_id is required when delivery_method is 'shipping'.");

                RuleFor(x => x.ShipTo)
                    .NotNull().WithMessage("ship_to is required when delivery_method is 'shipping'.");

                When(x => x.ShipTo != null, () =>
                {
                    RuleFor(x => x.ShipTo!.Country).GreaterThan(0).WithMessage("ship_to.country is required.");
                    RuleFor(x => x.ShipTo!.City).GreaterThan(0).WithMessage("ship_to.city is required.");
                    RuleFor(x => x.ShipTo!.GeoCoordinates)
                        .NotNull().WithMessage("ship_to.geo_coordinates is required.");
                });
            });

            When(x => x.DeliveryMethod == DeliveryMethodEnum.Pickup, () =>
            {
                RuleFor(x => x.BranchId)
                    .NotNull().GreaterThan(0)
                    .WithMessage("branch_id is required when delivery_method is 'pickup'.");
            });

            // Payment
            RuleFor(x => x.Payment).NotNull().WithMessage("payment is required.");

            When(x => x.Payment != null, () =>
            {
                RuleFor(x => x.Payment.Status)
                    .IsInEnum().WithMessage("payment.status must be 'paid' or 'pending_payment'.");

                When(x => x.Payment.Status == PaymentStatusEnum.Paid, () =>
                {
                    RuleFor(x => x.Payment.Method)
                        .NotNull().WithMessage("payment.method is required when payment.status is 'paid'.");
                });

                When(x => x.Payment.Status == PaymentStatusEnum.PendingPayment, () =>
                {
                    RuleFor(x => x.Payment.AcceptedMethods)
                        .NotNull().NotEmpty()
                        .WithMessage("payment.accepted_methods is required when payment.status is 'pending_payment'.");
                });

                When(x => x.Payment.Method == PaymentMethodEnum.Bank, () =>
                {
                    RuleFor(x => x.Payment.ReferenceId)
                        .NotNull().WithMessage("payment.reference_id is required when payment.method is 'bank'.");
                    RuleFor(x => x.Payment.ReceiptImagePath)
                        .NotEmpty().WithMessage("payment.receipt_image_path is required when payment.method is 'bank'.");
                });

                When(x => x.Payment.Method == PaymentMethodEnum.Subscription, () =>
                {
                    RuleFor(x => x.Payment.ReferenceId)
                        .NotNull().WithMessage("payment.reference_id is required when payment.method is 'subscription'.");
                    RuleFor(x => x.Payment.Recurring)
                        .Equal(true).WithMessage("payment.recurring must be true for subscription payments.");
                });

                When(x => x.Payment.Method == PaymentMethodEnum.Cod, () =>
                {
                    RuleFor(x => x.Payment.CashOnDelivery)
                        .NotNull().WithMessage("payment.cash_on_delivery is required when payment.method is 'cod'.");

                    When(x => x.Payment.CashOnDelivery != null, () =>
                    {
                        RuleFor(x => x.Payment.CashOnDelivery!.Amount).GreaterThan(0);
                        RuleFor(x => x.Payment.CashOnDelivery!.Currency).NotEmpty();
                    });
                });
            });

            // Products
            RuleFor(x => x.Products)
                .NotNull().NotEmpty().WithMessage("products must contain at least one item.");

            RuleForEach(x => x.Products).SetValidator(new CreateOrderProductValidator());
        }
    }

    public class CreateOrderProductValidator : AbstractValidator<CreateOrderProductDto>
    {
        public CreateOrderProductValidator()
        {
            RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("products[].quantity must be greater than 0.");
            RuleFor(i => i.IdentifierType).IsInEnum();
            RuleFor(i => i.Identifier).NotNull().WithMessage("products[].identifier is required.");
        }
    }
}
