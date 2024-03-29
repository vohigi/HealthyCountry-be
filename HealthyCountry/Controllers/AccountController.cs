﻿using System;
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
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserValidationContext _validationContext;
        public AccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
            _validationContext = new UserValidationContext();
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
            _validationContext.SetValidator(new RegisterUserRequestValidator());
            var validationResult = _validationContext.Validate(model);
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

        [HttpPut("{id}"), Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UserRequestDataModel model)
        {
            _validationContext.SetValidator(new UpdateUserRequestValidator());
            var validationResult = _validationContext.Validate(model);
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

        [HttpPut("{id}/status"), Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromQuery] bool status)
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
        public async Task<IActionResult> GetDoctors([FromQuery] string name, [FromQuery] DoctorSpecializations? spec,
            [FromQuery] Guid? orgId, [FromQuery] string sort, [FromQuery] int page = 1,
            [FromQuery] int limit = 30)
        {
            var (count, users) = await _userService.GetDoctors(name, page, limit, spec, orgId, sort);
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
        public IActionResult GetById([FromRoute] Guid id)
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