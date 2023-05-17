using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using HealthyCountry.DataModels;
using HealthyCountry.Models;
using HealthyCountry.Utilities;

namespace HealthyCountry.Services
{
    public interface IUserService
    {
        ServiceResponse<TokenDataModel, ValidationResult> Authenticate(string username, string password);
        IEnumerable<User> GetAll();

        Task<(int count, List<User>)> GetDoctors(string search, int page, int pageSize, DoctorSpecializations? spec,
            Guid? orgId, string sort);

        User GetById(Guid userId);
        Task<ServiceResponse<User, ValidationResult>> CreateUser(User userRequest);
        Task<ServiceResponse<User, ValidationResult>> UpdateUser(Guid id, User userRequest);
        Task<ServiceResponse<User, ValidationResult>> ChangeUserStatus(Guid id, bool status);
    }
}