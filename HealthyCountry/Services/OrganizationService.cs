using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using HealthyCountry.Models;
using HealthyCountry.Repositories;
using HealthyCountry.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HealthyCountry.Services
{
    public class OrganizationService:IOrganizationService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public OrganizationService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public async Task<ServiceResponse<Organization, ValidationResult>> CreateAsync(Organization organizationRequest)
        {
            var validationResult = new ValidationResult();
            var existingOrg = _dbContext.Organizations.SingleOrDefault(x => x.Edrpou == organizationRequest.Edrpou && x.IsActive);
            if (existingOrg != null)
            {
                validationResult.Errors.Add(new ValidationFailure("",
                    ValidationMessages.AlreadyExists));
                return new ServiceResponse<Organization, ValidationResult>(validationResult,
                    ServiceResponseStatuses.Conflict);
            }
            organizationRequest.Id = Guid.NewGuid();
            organizationRequest.IsActive = true;
            await _dbContext.Organizations.AddAsync(organizationRequest);
            await _dbContext.SaveChangesAsync();
            return new ServiceResponse<Organization, ValidationResult>(organizationRequest);
        }
        public IEnumerable<Organization> GetAll(string search = null)
        {
            if(string.IsNullOrEmpty(search))
                return _dbContext.Organizations.AsNoTracking().Where(x => x.IsActive).AsEnumerable();
            return _dbContext.Organizations.AsNoTracking().Where(x => x.IsActive && EF.Functions.Like(x.Name,"%"+search+"%")).AsEnumerable();
        }

        public Organization GetById(Guid organizationId)
        {
            var user = _dbContext.Organizations.Include(x=>x.Employees).AsNoTracking()
                .FirstOrDefault(x => x.Id == organizationId && x.IsActive);
            return user;
        }
    }
}