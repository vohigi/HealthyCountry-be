using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
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
    public class AccountController : Controller
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
        public IActionResult Login([FromBody] LoginRequestDataModel model)
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
        public async Task<IActionResult> Register([FromBody] UserRequestDataModel model)
        {
            var validator = new UserRequestDataModelValidator();
            var validationResult = validator.Validate(model, ruleSet:"register");
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
        [HttpPut("{id}"),Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateUser([FromRoute]string id,[FromBody] UserRequestDataModel model)
        {
            var validationResult = await new UserRequestDataModelValidator().ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState, null);
                return StatusCode(400, ModelHelpers.DecorateModelState(ModelState));
            }

            var user = _mapper.Map<User>(model);
            var createUserServiceResponse = await _userService.UpdateUser(id, user);

            if (!createUserServiceResponse.IsSuccess)
            {
                createUserServiceResponse.Errors.AddToModelState(ModelState, null);
                return StatusCode(createUserServiceResponse.Status.ToHttpStatusCode(),
                    ModelHelpers.DecorateModelState(ModelState, createUserServiceResponse.Status.GetDescription()));
            }

            return Ok(createUserServiceResponse.Result);
        }
        [HttpPut("{id}/status"),Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateUser([FromRoute]string id,[FromQuery] bool status)
        {
            
            var createUserServiceResponse = await _userService.ChangeUserStatus(id, status);

            if (!createUserServiceResponse.IsSuccess)
            {
                createUserServiceResponse.Errors.AddToModelState(ModelState, null);
                return StatusCode(createUserServiceResponse.Status.ToHttpStatusCode(),
                    ModelHelpers.DecorateModelState(ModelState, createUserServiceResponse.Status.GetDescription()));
            }

            return Ok(createUserServiceResponse.Result);
        }

        [HttpGet("doctors"), Authorize(Roles = "PATIENT,ADMIN")]
        public async Task<IActionResult> GetDoctors([FromQuery] string name, [FromQuery] int page = 1,
            [FromQuery] int limit = 30)
        {
            var (count, users) = await _userService.GetDoctors(name, page, limit);
            var response = new ResponseData(users);
            response.AddPaginationData(count, page, limit);
            return Ok(response);
        }

        [HttpGet("all"), Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAll([FromQuery] string name, [FromQuery] int page = 1,
            [FromQuery] int limit = 30)
        {
            var users = _userService.GetAll();
            var response = new ResponseData(users);
            //response.AddPaginationData(count, page, limit);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] string id)
        {
            // only allow admins to access other user records
            // var currentUserId = User.Identity.Name;
            // if (id != currentUserId && !User.IsInRole(UserRoles.ADMIN.ToString()))
            //     return Forbid();

            var user = _userService.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}