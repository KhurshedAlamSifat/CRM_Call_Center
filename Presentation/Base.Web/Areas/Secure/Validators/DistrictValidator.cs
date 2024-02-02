using Base.Web.Areas.Secure.Models.Districts;
using FluentValidation;

namespace Base.Web.Areas.Secure.Validators
{
    public class DistrictValidator : AbstractValidator<DistrictModel>
    {
        public DistrictValidator() 
        {
            RuleFor(model=>model.DistrictName)
                .NotEmpty().WithMessage("District name is required.");
        }
    }
}
