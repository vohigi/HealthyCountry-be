using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using HealthyCountry.Models;
using HealthyCountry.Utilities;

namespace HealthyCountry.Services
{
    public interface IOrganizationService
    {
        Task<ServiceResponse<Organization, ValidationResult>> CreateAsync(Organization organizationRequest);
        IEnumerable<Organization> GetAll(string search = null);
        Organization GetById(string organizationId);
    }
}