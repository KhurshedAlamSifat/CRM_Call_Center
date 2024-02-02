using Base.Web.Areas.Secure.Models.Category;
using FluentValidation;

namespace Base.Web.Areas.Secure.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryModel>
    {
        public CategoryValidator() 
        {
            RuleFor(model=>model.CategoryName)
                .NotEmpty().WithMessage("Category name is required.");

            RuleFor(model => model.Brand_Id)
                .NotEmpty().WithMessage("Brand name is required.")
                .Must(id => id != -1).WithMessage("Please select a valid Brand.");
        }
    }
}
