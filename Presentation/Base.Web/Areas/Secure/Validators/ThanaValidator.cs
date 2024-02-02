using Base.Web.Areas.Secure.Models.Thanas;
using FluentValidation;

namespace Base.Web.Areas.Secure.Validators
{
    public class ThanaValidator : AbstractValidator<ThanaModel>
    {
        public ThanaValidator() 
        {
            RuleFor(model=>model.ThanaName)
                .NotEmpty().WithMessage("Thana name is required.");
            RuleFor(model=>model.District_Id)
                .NotEmpty().WithMessage("District name is required.")
                .Must(id => id != -1).WithMessage("Please select a valid district."); 
        }

    }
}
