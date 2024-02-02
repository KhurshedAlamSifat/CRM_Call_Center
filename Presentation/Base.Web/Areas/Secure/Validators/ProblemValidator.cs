using Base.Web.Areas.Secure.Models.Problems;
using FluentValidation;

namespace Base.Web.Areas.Secure.Validators
{
    public class ProblemValidator : AbstractValidator<ProblemTypeModel>
    {
        public ProblemValidator()
        {
            RuleFor(model => model.ProblemDescription)
                .NotEmpty().WithMessage("Problem describtion is required.");
            RuleFor(model => model.CategoryId)
                .NotEmpty().WithMessage("Category Name is required.")
                .Must(id => id != -1).WithMessage("Please select a valid Category.");
        }
    }
}
