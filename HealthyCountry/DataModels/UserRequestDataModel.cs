using System;
using FluentValidation;
using HealthyCountry.Models;

namespace HealthyCountry.DataModels
{
    public class UserRequestDataModel
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string TaxId { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string OrganizationId { get; set; }
    }

    public class UserRequestDataModelValidator : AbstractValidator<UserRequestDataModel>
    {
        public UserRequestDataModelValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            RuleFor(x => x.FirstName)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Please specify a first name");
            RuleFor(x => x.Gender)
                .NotEmpty();
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(@"(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
            RuleFor(x => x.Role)
                .IsEnumName(typeof(UserRoles));
        }
    }
}