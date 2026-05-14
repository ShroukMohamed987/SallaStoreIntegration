using FluentValidation;
using SallaStoreIntegration.Dtos.Orders;

namespace SallaStoreIntegration.Validation
{
    public class UpdateBulkOrdersStatusesValidator : AbstractValidator<UpdateBulkOrdersStatusesFormDto>
    {
        public UpdateBulkOrdersStatusesValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("file is required.")
                .Must(f => f != null && f.Length > 0).WithMessage("file cannot be empty.")
                .Must(f => f != null && f.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                .WithMessage("file must be an .xlsx Excel file.");
        }
    }
}
