using FluentValidation.Results;
using HealthyCountry.DataModels;

namespace HealthyCountry.Services
{
    public class UserValidationContext
    {
        private IUserValidator _validator;

        public void SetValidator(IUserValidator validator)
        {
            _validator = validator;
        }

        public ValidationResult Validate(UserRequestDataModel userRequest)
        {
            return _validator.Validate(userRequest);
        }
    }
}