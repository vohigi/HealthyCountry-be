using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using HealthyCountry.DataModels;
using HealthyCountry.Models;
using HealthyCountry.Services;
using HealthyCountry.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyCountry.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class AccountController:Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginRequestDataModel model)
        {
            var loginServiceResponse = _userService.Authenticate(model.Email, model.Password);

            if (!loginServiceResponse.IsSuccess)
            {
                loginServiceResponse.Errors.AddToModelState(ModelState, null);
                return StatusCode(loginServiceResponse.Status.ToHttpStatusCode(),
                    ModelHelpers.DecorateModelState(ModelState, loginServiceResponse.Status.GetDescription()));
            }

            return Ok(loginServiceResponse.Result);
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRequestDataModel model)
        {
            var validationResult = await new UserRequestDataModelValidator().ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return StatusCode(400, ModelHelpers.DecorateModelState(ModelState));
            }
            var user = _mapper.Map<User>(model);
            var createUserServiceResponse = await _userService.CreateUser(user);

            if (!createUserServiceResponse.IsSuccess)
            {
                createUserServiceResponse.Errors.AddToModelState(ModelState, null);
                return StatusCode(createUserServiceResponse.Status.ToHttpStatusCode(),
                    ModelHelpers.DecorateModelState(ModelState, createUserServiceResponse.Status.GetDescription()));
            }

            return Ok(createUserServiceResponse.Result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users =  _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            // only allow admins to access other user records
            var currentUserId = User.Identity.Name;
            if (id != currentUserId && !User.IsInRole(UserRoles.ADMIN.ToString()))
                return Forbid();

            var user =  _userService.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}