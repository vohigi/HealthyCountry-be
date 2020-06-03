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
        Task<(int count, IEnumerable<User>)> GetDoctors(string search, int page, int count);
        User GetById(string userId);
        Task<ServiceResponse<User, ValidationResult>> CreateUser(User userRequest);
        Task<ServiceResponse<User, ValidationResult>> UpdateUser(string id, User userRequest);
        Task<ServiceResponse<User, ValidationResult>> ChangeUserStatus(string id, bool status);
    }
}