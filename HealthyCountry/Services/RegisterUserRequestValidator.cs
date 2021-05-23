using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using HealthyCountry.DataModels;
using HealthyCountry.Models;

namespace HealthyCountry.Services
{
    public class RegisterUserRequestValidator : AbstractValidator<UserRequestDataModel>, IUserValidator
    {
        public new ValidationResult Validate(UserRequestDataModel userRequest)
        {
            return base.Validate(userRequest);
        }

        public RegisterUserRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.FirstName)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .NotEmpty();
            RuleFor(x => x.Gender)
                .NotEmpty();
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
            RuleFor(x => x.Role)
                .NotEmpty()
                .IsEnumName(typeof(UserRoles));
        }
    }
}