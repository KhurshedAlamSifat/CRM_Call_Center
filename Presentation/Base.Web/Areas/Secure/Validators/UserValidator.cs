using Base.Web.Areas.Secure.Models.Users;
using FluentValidation;

namespace Base.Web.Areas.Secure.Validators
{
    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator() 
        {   
            RuleFor(model=>model.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(255).WithMessage("Username cannot exceed 255 characters.");

            RuleFor(model=>model.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(model => model.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(model => model.Gender)
                .NotEmpty().WithMessage("Gender is required.");

            RuleFor(model => model.Company)
                .NotEmpty().WithMessage("Company is required.")
                .MaximumLength(100).WithMessage("Company name cannot exceed 100 characters.");

            RuleFor(model => model.HomeAddress)
                .NotEmpty().WithMessage("Home address is required.")
                .MaximumLength(255).WithMessage("Home address cannot exceed 255 characters.");

            RuleFor(model => model.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^01\d{9}$").WithMessage("Invalid phone number format. It should start with '01' ");
        }
    }
}
