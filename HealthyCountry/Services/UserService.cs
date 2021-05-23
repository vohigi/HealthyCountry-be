using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using HealthyCountry.DataModels;
using HealthyCountry.Models;
using HealthyCountry.Repositories;
using HealthyCountry.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HealthyCountry.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public UserService(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public ServiceResponse<TokenDataModel, ValidationResult> Authenticate(string email, string password)
        {
            var validationResult = new ValidationResult();
            var user = _dbContext.Users.Include(x => x.Organization).SingleOrDefault(x => x.Email == email);

            // return null if user not found
            if (user == null)
            {
                validationResult.Errors.Add(new ValidationFailure("",
                    ValidationMessages.Unauthorized));
                return new ServiceResponse<TokenDataModel, ValidationResult>(validationResult,
                    ServiceResponseStatuses.Unauthorized);
            }

            var a = BCrypt.Net.BCrypt.HashPassword("Sasha280920");
            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                validationResult.Errors.Add(new ValidationFailure("",
                    ValidationMessages.Unauthorized));
                return new ServiceResponse<TokenDataModel, ValidationResult>(validationResult,
                    ServiceResponseStatuses.Unauthorized);
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("auth:key").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires =
                    DateTime.UtcNow.AddMinutes(Int32.Parse(_configuration.GetSection("auth:tokenLifeTime").Value)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var result = new TokenDataModel {User = user, Token = tokenHandler.WriteToken(token)};
            return new ServiceResponse<TokenDataModel, ValidationResult>(result);
        }

        public async Task<ServiceResponse<User, ValidationResult>> CreateUser(User userRequest)
        {
            var validationResult = new ValidationResult();
            var existingUser = _dbContext.Users.SingleOrDefault(x => x.Email == userRequest.Email && x.IsActive);
            if (existingUser != null)
            {
                validationResult.Errors.Add(new ValidationFailure("",
                    ValidationMessages.AlreadyExists));
                return new ServiceResponse<User, ValidationResult>(validationResult,
                    ServiceResponseStatuses.Conflict);
            }

            userRequest.Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            userRequest.UserId = Guid.NewGuid().ToString();
            userRequest.IsActive = true;
            await _dbContext.Users.AddAsync(userRequest);
            await _dbContext.SaveChangesAsync();
            return new ServiceResponse<User, ValidationResult>(userRequest);
        }

        public async Task<ServiceResponse<User, ValidationResult>> UpdateUser(string id, User userRequest)
        {
            var validationResult = new ValidationResult();
            var existingUser = _dbContext.Users.SingleOrDefault(x => x.UserId == id);
            if (existingUser == null)
            {
                validationResult.Errors.Add(new ValidationFailure("",
                    ValidationMessages.EntityNotFound));
                return new ServiceResponse<User, ValidationResult>(validationResult,
                    ServiceResponseStatuses.NotFound);
            }

            ModelHelpers.Merge(existingUser, userRequest);
            existingUser.Password = string.IsNullOrEmpty(userRequest.Password)
                ? existingUser.Password
                : BCrypt.Net.BCrypt.HashPassword(userRequest.Password);
            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync();
            return new ServiceResponse<User, ValidationResult>(existingUser);
        }

        public async Task<ServiceResponse<User, ValidationResult>> ChangeUserStatus(string id, bool status)
        {
            var validationResult = new ValidationResult();
            var existingUser = _dbContext.Users.SingleOrDefault(x => x.UserId == id);
            if (existingUser == null)
            {
                validationResult.Errors.Add(new ValidationFailure("",
                    ValidationMessages.EntityNotFound));
                return new ServiceResponse<User, ValidationResult>(validationResult,
                    ServiceResponseStatuses.NotFound);
            }

            existingUser.IsActive = status;
            _dbContext.Users.Update(existingUser);
            await _dbContext.SaveChangesAsync();
            return new ServiceResponse<User, ValidationResult>(existingUser);
        }

        public IEnumerable<User> GetAll()
        {
            return _dbContext.Users.Include(x => x.Organization).AsNoTracking().AsEnumerable();
        }

        public async Task<(int count, List<User>)> GetDoctors(string search, int page, int pageSize, DoctorSpecializations? spec,
            string orgId, string sort)
        {
            var request = _dbContext.Users.Include(d => d.Organization).Where(d =>
                (string.IsNullOrEmpty(search) || EF.Functions.Like(d.LastName, "%" + search + "%")) &&
                    (string.IsNullOrEmpty(orgId) || d.OrganizationId == orgId) && 
                    (!spec.HasValue || d.Specialization == spec) && 
                    (d.Role == UserRoles.DOCTOR || d.Role == UserRoles.ADMIN));

            var count = await request.CountAsync();
            switch (sort)
            {
                case "lastName_asc":
                    request = request.OrderBy(s => s.LastName);
                    break;
                case "lastName_desc":
                    request = request.OrderByDescending(s => s.LastName);
                    break;
                default:
                    request = request.OrderBy(s => s.LastName);
                    break;
            }
            var items =  request.Skip((page - 1) * pageSize).Take(pageSize);
            
            return (count, items.ToList());
        }

        public User GetById(string userId)
        {
            var user = _dbContext.Users.Include(x => x.Organization).AsNoTracking()
                .FirstOrDefault(x => x.UserId == userId);
            return user;
        }
    }
}