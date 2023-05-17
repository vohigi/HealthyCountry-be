using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using HealthyCountry.Models;
using HealthyCountry.Services;
using HealthyCountry.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyCountry.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/organizations")]
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly IMapper _mapper;

        public OrganizationController(IOrganizationService organizationService, IMapper mapper)
        {
            _organizationService = organizationService;
            _mapper = mapper;
        }
        
        // GET
        [HttpGet]
        public IActionResult GetOrganizationsAsync([FromQuery] string name)
        {
            var res = _organizationService.GetAll(name);
            return Ok(res);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetOrganizationAsync([FromRoute] Guid id)
        {
            var res = _organizationService.GetById(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Organization organization)
        {
            var createOrgServiceResponse = await _organizationService.CreateAsync(organization);

            if (!createOrgServiceResponse.IsSuccess)
            {
                createOrgServiceResponse.Errors.AddToModelState(ModelState, null);
                return StatusCode(createOrgServiceResponse.Status.ToHttpStatusCode(),
                    ModelHelpers.DecorateModelState(ModelState, createOrgServiceResponse.Status.GetDescription()));
            }

            return Ok(createOrgServiceResponse.Result);
        }
    }
}