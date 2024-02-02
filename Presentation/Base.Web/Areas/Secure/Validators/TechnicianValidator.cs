using Base.Web.Areas.Secure.Models.Technicians;
using FluentValidation;

namespace Base.Web.Areas.Secure.Validators
{
    public class TechnicianValidator : AbstractValidator<TechnicianModel>
    {
        public TechnicianValidator()
        {
            RuleFor(model=>model.TechnicianName)
                .NotEmpty().WithMessage("Technician name is required");
        }
    }
}
