using Base.Web.Areas.Secure.Models.ComplainTypes;
using FluentValidation;

namespace Base.Web.Areas.Validatiors
{
    public class ComplainTypeValidator : AbstractValidator<ComplainTypeModel>
    {
        public ComplainTypeValidator()
        {
            RuleFor(model => model.ComplainTypeName)
            .NotEmpty().WithMessage("Complain type name is required.")
            .MaximumLength(255).WithMessage("Complain type name cannot exceed 255 characters.");
        }
    }
}
