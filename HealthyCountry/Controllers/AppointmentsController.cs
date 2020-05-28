using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HealthyCountry.Repositories;
using HealthyCountry.Services;
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
        private readonly IUserService _appointmentsService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public AppointmentsController(IUserService appointmentsService, IMapper mapper, ApplicationDbContext dbContext)
        {
            _appointmentsService = appointmentsService;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var userID = User.Identity.Name;
            var user = _dbContext.Users.Include(x => x.Organization).AsNoTracking()
                .FirstOrDefault(x => x.UserId == userID);
            // AppointmentsViewModel model = new AppointmentsViewModel();
            // model.User = user;
            DateTime dt = DateTime.Now;
            DateTime dateDefault = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            DateTime[] dates = new DateTime[]
            {
                new DateTime(dt.Year, dt.Month, dt.Day + 1, 9, 0, 0),
                new DateTime(dt.Year, dt.Month, dt.Day + 1, 10, 0, 0),
                new DateTime(dt.Year, dt.Month, dt.Day + 1, 11, 0, 0),
                new DateTime(dt.Year, dt.Month, dt.Day + 1, 12, 0, 0)
            };

            var appointments = _dbContext.Appointments.Include(x => x.Employee)
                .Where(x => x.EmployeeId == user.UserId)
                .OrderByDescending(x => x.DateTime);


            if (appointments.ToList().Count == 0)
            {
                List<Appointment> appList = new List<Appointment>()
                {
                    new Appointment(userID, dates[0], AppointmentStatuses.FREE),
                    new Appointment(userID, dates[1], AppointmentStatuses.FREE),
                    new Appointment(userID, dates[2], AppointmentStatuses.FREE),
                    new Appointment(userID, dates[3], AppointmentStatuses.FREE),
                    new Appointment(userID, dates[0].AddDays(1), AppointmentStatuses.FREE),
                    new Appointment(userID, dates[1].AddDays(1), AppointmentStatuses.FREE),
                    new Appointment(userID, dates[2].AddDays(1), AppointmentStatuses.FREE),
                    new Appointment(userID, dates[3].AddDays(1), AppointmentStatuses.FREE)
                };
                await _dbContext.Appointments.AddRangeAsync(appList);
                await _dbContext.SaveChangesAsync();
            }
            else if ((appointments.ToList()[0].DateTime - dt).TotalDays > 0)
            {
                var a = (appointments.ToList()[0].DateTime - dt).TotalDays;
                if ((appointments.ToList()[0].DateTime - dt).TotalDays <= 1)
                {
                    List<Appointment> appList = new List<Appointment>()
                    {
                        new Appointment(userID, dates[0].AddDays(1), AppointmentStatuses.FREE),
                        new Appointment(userID, dates[1].AddDays(1), AppointmentStatuses.FREE),
                        new Appointment(userID, dates[2].AddDays(1), AppointmentStatuses.FREE),
                        new Appointment(userID, dates[3].AddDays(1), AppointmentStatuses.FREE)
                    };
                    await _dbContext.Appointments.AddRangeAsync(appList);
                    await _dbContext.SaveChangesAsync();
                }
            }
            else if (appointments.ToList()[0].DateTime <= dt)
            {
                List<Appointment> appList = new List<Appointment>()
                {
                    new Appointment(userID, dates[0], AppointmentStatuses.FREE),
                    new Appointment(userID, dates[1], AppointmentStatuses.FREE),
                    new Appointment(userID, dates[2], AppointmentStatuses.FREE),
                    new Appointment(userID, dates[3], AppointmentStatuses.FREE),
                    new Appointment(userID, dates[0].AddDays(1), AppointmentStatuses.FREE),
                    new Appointment(userID, dates[1].AddDays(1), AppointmentStatuses.FREE),
                    new Appointment(userID, dates[2].AddDays(1), AppointmentStatuses.FREE),
                    new Appointment(userID, dates[3].AddDays(1), AppointmentStatuses.FREE)
                };
                await _dbContext.Appointments.AddRangeAsync(appList);
                await _dbContext.SaveChangesAsync();
            }

            var appointmentsCurrent = _dbContext.Appointments.AsNoTracking().Where(x => x.EmployeeId == userID && x.DateTime.Date >= dt.Date)
                .OrderBy(x => x.DateTime);
            //model.Appointment = 
                return Ok(appointmentsCurrent.ToList());
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody]Appointment model)
        {
            model.AppointmentId = id;
            _dbContext.Appointments.Update(model);
            await _dbContext.SaveChangesAsync();
            return Ok(model);
        }
    }
}