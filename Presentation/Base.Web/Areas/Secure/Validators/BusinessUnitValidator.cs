using Base.Web.Areas.Secure.Models.BusinessUnit;
using FluentValidation;

namespace Base.Web.Areas.Secure.Validators
{
    public class BusinessUnitValidator : AbstractValidator<BusinessUnitModel>
    {
        public BusinessUnitValidator()
        {
            RuleFor(model=>model.BusinessUnitName)
                .NotEmpty().WithMessage("Business unit name is required.")
            .MaximumLength(255).WithMessage("Business unit name cannot exceed 255 characters.");
        }
    }
}
