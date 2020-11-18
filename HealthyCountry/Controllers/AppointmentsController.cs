﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using HealthyCountry.Models;
using HealthyCountry.Repositories;
using HealthyCountry.Services;
using HealthyCountry.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MIS.Models;

namespace HealthyCountry.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        private readonly GetAppointmentsFacadeSingleton _appointmentsFacade;

        public AppointmentsController(IUserService userService, IMapper mapper, ApplicationDbContext dbContext)
        {
            _userService = userService;
            _mapper = mapper;
            _dbContext = dbContext;
            _appointmentsFacade = new GetAppointmentsFacadeSingleton();
        }

        [HttpGet("{appId}/one")]
        public async Task<IActionResult> GetOneAsync([FromRoute] string appId)
        {
            var result= _dbContext.Appointments.Include(x => x.Employee).Include(x => x.Action).Include(x => x.Reason)
                .Include(x => x.Diagnosis).FirstOrDefault(x => x.AppointmentId == appId);
            return Ok(result);
        }
        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetAsync([FromRoute] string doctorId)
        {
            var user = _dbContext.Users.Include(x => x.Organization).AsNoTracking()
                .FirstOrDefault(x => x.UserId == doctorId);
            DateTime dt = DateTime.Now;
            var facade = _appointmentsFacade.GetFacade(_dbContext);
            var result = await facade.GetAppointmentsAsync(user.UserId, dt);
            return Ok(result);
        }
        [HttpGet("patient/{patientId}")]
        public IActionResult GetByPatientAsync([FromRoute] string patientId)
        {
            var user = _userService.GetById(patientId);
            if (user == null)
            {
                return NotFound();
            }
            var appointments = _dbContext.Appointments.Include(x => x.Employee).Include(x => x.Action).Include(x => x.Reason)
                .Include(x => x.Diagnosis)
                .Where(x => x.PatientId == user.UserId)
                .OrderByDescending(x => x.DateTime).ToList();
            return Ok(appointments);
        }
        [HttpGet("doctor/{doctorId}")]
        public IActionResult GetByDoctorAsync([FromRoute] string doctorId)
        {
            var user = _userService.GetById(doctorId);
            if (user == null)
            {
                return NotFound();
            }

            var appointments = _dbContext.Appointments.Include(x => x.Employee).Include(x => x.Action).Include(x => x.Reason)
                .Include(x => x.Diagnosis)
                .Where(x => x.EmployeeId == user.UserId && x.Status != AppointmentStatuses.FREE)
                .OrderByDescending(x => x.DateTime).ToList();
            return Ok(appointments);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] Appointment model)
        {
            model.AppointmentId = id;
            model.Employee = null;
            _dbContext.Appointments.Update(model);
            await _dbContext.SaveChangesAsync();
            return Ok(model);
        }
        [AllowAnonymous]
        [HttpGet("icpc2")]
        public async Task<IActionResult> GetAsync(
            [FromQuery(Name = "search")] string searchRequest,
            [FromQuery(Name = "ids")] string idsRequest,
            [FromQuery] ICPC2Groups? groupId,
            [FromQuery] bool? isActive,
            [FromQuery] int page = 1,
            [FromQuery] int? limit = 30)
        {
            var ids = new HashSet<string>();
            if (!string.IsNullOrEmpty(idsRequest))
            {
                ids = idsRequest.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .ToHashSet();
            }

            var elements = await GetCodesAsync(searchRequest, ids, groupId, isActive, page, limit);
            var response = new ResponseData(elements.data);
            response.AddPaginationData(elements.count, page, limit.Value);
            return Ok(response);
        }

        public async Task<(int count, List<ICPC2Entity> data)> GetCodesAsync(string search,
            HashSet<string> ids, ICPC2Groups? groupId = null, bool? isActual = null, int page = 1,
            int? limit = null)
        {
            var requestBase = _dbContext.ICPC2Codes.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                var fullTextQuery = "SELECT * FROM ICPC2";
                var matchQuery = Regex.Replace(search, "[^\\w\\._* ]", " ", RegexOptions.Compiled);
                var tokens = matchQuery.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(t => t.Length > 2)
                    .ToArray();
                fullTextQuery = $"{fullTextQuery} where Code like ('{matchQuery}%') or NumberOnlyCode = '{matchQuery}'";
                if (tokens.Any())
                    fullTextQuery += $" OR MATCH (name) AGAINST ('+{string.Join("* +", tokens)}*' in boolean mode)";
                requestBase = _dbContext.ICPC2Codes.FromSqlRaw(fullTextQuery);
            }

            var request = requestBase
                .Include(i => i.Groups)
                .AsNoTracking()
                .Where(i =>
                    (ids.Count == 0 || ids.Contains(i.Id))
                    && (!isActual.HasValue || i.IsActual == isActual)
                    && (!groupId.HasValue || i.Groups.Any(g => g.GroupId == groupId.Value)))
                .OrderBy(d => d.Code);

            var counter = await request.CountAsync();

            var dataFetch = await (limit.HasValue
                ? request.Skip((page - 1) * limit.Value).Take(limit.Value)
                : request).ToListAsync();

            if (counter == 0) return (counter, dataFetch);
            return (counter, dataFetch);
        }
    }
}
 // DateTime dateDefault = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
 //            DateTime[] dates = new DateTime[]
 //            {
 //                dateDefault.AddDays(1).AddHours(9),
 //                dateDefault.AddDays(1).AddHours(10),
 //                dateDefault.AddDays(1).AddHours(11),
 //                dateDefault.AddDays(1).AddHours(12),
 //            };
 //
 //            var appointments = _dbContext.Appointments.Include(x => x.Employee)
 //                .Where(x => x.EmployeeId == user.UserId)
 //                .OrderByDescending(x => x.DateTime).ToList();
 //
 //
 //            if (appointments.Count == 0)
 //            {
 //                List<Appointment> appList = new List<Appointment>()
 //                {
 //                    new Appointment(doctorId, dates[0], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[1], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[2], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[3], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[0].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[1].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[2].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[3].AddDays(1), AppointmentStatuses.FREE)
 //                };
 //                await _dbContext.Appointments.AddRangeAsync(appList);
 //                await _dbContext.SaveChangesAsync();
 //            }
 //            else if ((appointments[0].DateTime - dt).TotalDays > 0)
 //            {
 //                var a = (appointments[0].DateTime - dt).TotalDays;
 //                if ((appointments[0].DateTime - dt).TotalDays <= 1)
 //                {
 //                    List<Appointment> appList = new List<Appointment>()
 //                    {
 //                        new Appointment(doctorId, dates[0].AddDays(1), AppointmentStatuses.FREE),
 //                        new Appointment(doctorId, dates[1].AddDays(1), AppointmentStatuses.FREE),
 //                        new Appointment(doctorId, dates[2].AddDays(1), AppointmentStatuses.FREE),
 //                        new Appointment(doctorId, dates[3].AddDays(1), AppointmentStatuses.FREE)
 //                    };
 //                    await _dbContext.Appointments.AddRangeAsync(appList);
 //                    await _dbContext.SaveChangesAsync();
 //                }
 //            }
 //            else if (appointments[0].DateTime <= dt)
 //            {
 //                List<Appointment> appList = new List<Appointment>()
 //                {
 //                    new Appointment(doctorId, dates[0], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[1], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[2], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[3], AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[0].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[1].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[2].AddDays(1), AppointmentStatuses.FREE),
 //                    new Appointment(doctorId, dates[3].AddDays(1), AppointmentStatuses.FREE)
 //                };
 //                await _dbContext.Appointments.AddRangeAsync(appList);
 //                await _dbContext.SaveChangesAsync();
 //            }
 //
 //            var canceled = appointments.Where(x => x.Status == AppointmentStatuses.CANCELED).ToList();
 //            foreach (var appointment in canceled)
 //            {
 //                if (!_dbContext.Appointments.Any(x => x.EmployeeId == appointment.EmployeeId && x.DateTime == appointment.DateTime &&
 //                                                     x.Status != AppointmentStatuses.CANCELED))
 //                {
 //                    await _dbContext.Appointments.AddAsync(new Appointment(appointment.EmployeeId, appointment.DateTime,
 //                        AppointmentStatuses.FREE));
 //                }
 //            }
 //            
 //            await _dbContext.SaveChangesAsync();
 //
 //            var appointmentsCurrent = _dbContext.Appointments.AsNoTracking()
 //                .Where(x => x.EmployeeId == doctorId && x.DateTime.Date >= dt.Date && x.Status!=AppointmentStatuses.CANCELED)
 //                .OrderBy(x => x.DateTime).ToList();