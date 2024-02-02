using FluentValidation;

namespace Base.Web.Framework.Validators
{
    /// <summary>
    /// Validator extensions
    /// </summary>
    public static class ValidatorExtensions
    {

        /// <summary>
        /// Implement password validator
        /// </summary>
        /// <typeparam name="TModel">Type of model being validated</typeparam>
        /// <param name="ruleBuilder">Rule builder</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="customerSettings">Customer settings</param>
        /// <returns>Result</returns>
        public static IRuleBuilder<TModel, string> IsPassword<TModel>(this IRuleBuilder<TModel, string> ruleBuilder)
        {
            var regExp = "^";
            //Passwords must be at least X characters and contain the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*-)
            //regExp += customerSettings.PasswordRequireUppercase ? "(?=.*?[A-Z])" : "";
            //regExp += customerSettings.PasswordRequireLowercase ? "(?=.*?[a-z])" : "";
            //regExp += customerSettings.PasswordRequireDigit ? "(?=.*?[0-9])" : "";
            //regExp += customerSettings.PasswordRequireNonAlphanumeric ? "(?=.*?[#?!@$%^&*-])" : "";

            regExp += $".{{{6},}}$";

            var options = ruleBuilder
                .NotEmpty().WithMessage("Password can not be empty")
                .Matches(regExp).WithMessage("Password need to be 6 digits");

            return options;
        }
    }
}