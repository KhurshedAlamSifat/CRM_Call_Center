using Base.Web.Areas.Secure.Models.Brand;
using FluentValidation;

namespace Base.Web.Areas.Secure.Validators
{
    public class BrandValidator : AbstractValidator<BrandModel>
    {
        public BrandValidator()
        {
            RuleFor(model => model.BrandName)
                .NotNull().WithMessage("Brand name is required.");
            RuleFor(model => model.BusinessUnit_Id)
                .NotEmpty().WithMessage("Business unit name is required.")
                .Must(id => id != -1).WithMessage("Please select a valid Business Unit.");
        } 
        
    }
}
