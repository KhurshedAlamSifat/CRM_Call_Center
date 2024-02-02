using Base.Web.Areas.Secure.Models.ServiceCenter;
using FluentValidation;

namespace Base.Web.Areas.Secure.Validators
{
    public class ServiceCenterValidator : AbstractValidator<ServiceCenterModel>
    {
        public ServiceCenterValidator() 
        {
            RuleFor(model => model.ServiceCenterName)
                .NotEmpty().WithMessage("Service centername is required");
            RuleFor(model => model.Thana_Id)
                .NotEmpty().WithMessage("Thana name is required")
                .Must(id => id != -1).WithMessage("Please select a valid thana.");
        }

    }
}
