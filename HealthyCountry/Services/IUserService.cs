﻿using System.Collections.Generic;
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
        User GetById(string userId);
        Task<ServiceResponse<User, ValidationResult>> CreateUser(User userRequest);
    }
}