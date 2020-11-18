using FluentValidation.Results;
using HealthyCountry.DataModels;
using Microsoft.Extensions.Options;

namespace HealthyCountry.Services
{
    public interface IUserValidator
    {
        ValidationResult Validate(UserRequestDataModel userRequest);
    }
}