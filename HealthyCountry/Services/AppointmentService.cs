using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyCountry.Models;
using HealthyCountry.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthyCountry.Services
{
    public class AppointmentService : IAppointmentFacade
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Appointment>> GetAppointmentsAsync(string doctorId, DateTime dateFrom)
        {
            var numberOfSlotsToCreate = GetNumberOfSlotsToCreate(doctorId, dateFrom);
            if (numberOfSlotsToCreate != 0)
            {
                CreateAppointmentSlots(doctorId, dateFrom, numberOfSlotsToCreate);
            }

            CreateSlotsForCancelledAppointments(doctorId, dateFrom);
            return _dbContext.Appointments.AsNoTracking()
                .Where(x => x.EmployeeId == Guid.Parse(doctorId) &&
                            x.DateTime.Date >= DateTime.SpecifyKind(dateFrom.Date, DateTimeKind.Utc) &&
                            x.Status != AppointmentStatuses.CANCELED)
                .OrderBy(x => x.DateTime).ToListAsync();
        }

        private void CreateAppointmentSlots(string doctorId, DateTime dateFrom, int numberOfSlotsToCreate)
        {
            var employeeId = Guid.Parse(doctorId);
            DateTime dateDefault = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0, DateTimeKind.Utc);
            DateTime[] dates = new DateTime[]
            {
                dateDefault.AddDays(1).AddHours(9),
                dateDefault.AddDays(1).AddHours(10),
                dateDefault.AddDays(1).AddHours(11),
                dateDefault.AddDays(1).AddHours(12),
            };
            List<Appointment> appointmentsList = new List<Appointment>();
            switch (numberOfSlotsToCreate)
            {
                case 1:
                    appointmentsList.Add(new Appointment(employeeId, dates[0].AddDays(1), AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[1].AddDays(1), AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[2].AddDays(1), AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[3].AddDays(1), AppointmentStatuses.FREE));
                    break;
                case 2:
                    appointmentsList.Add(new Appointment(employeeId, dates[0], AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[1], AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[2], AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[3], AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[0].AddDays(1), AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[1].AddDays(1), AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[2].AddDays(1), AppointmentStatuses.FREE));
                    appointmentsList.Add(new Appointment(employeeId, dates[3].AddDays(1), AppointmentStatuses.FREE));
                    break;
            }

            if (appointmentsList.Count > 0)
            {
                _dbContext.Appointments.AddRange(appointmentsList);
                _dbContext.SaveChanges();
            }
        }

        private void CreateSlotsForCancelledAppointments(string doctorId, DateTime dateFrom)
        {
            var appointments = _dbContext.Appointments.Include(x => x.Employee)
                .Where(x => x.EmployeeId == Guid.Parse(doctorId))
                .OrderByDescending(x => x.DateTime).ToList();
            var canceled = appointments.Where(x => x.Status == AppointmentStatuses.CANCELED).ToList();
            foreach (var appointment in canceled)
            {
                if (!_dbContext.Appointments.Any(x => x.EmployeeId == appointment.EmployeeId && x.DateTime == appointment.DateTime.ToUniversalTime() &&
                                                      x.Status != AppointmentStatuses.CANCELED))
                {
                    _dbContext.Appointments.Add(new Appointment(appointment.EmployeeId, appointment.DateTime,
                        AppointmentStatuses.FREE));
                }
            }
            _dbContext.SaveChanges();
        }
        private int GetNumberOfSlotsToCreate(string doctorId, DateTime dateFrom)
        {
            var appointments = _dbContext.Appointments.Include(x => x.Employee)
                .Where(x => x.EmployeeId == Guid.Parse(doctorId))
                .OrderByDescending(x => x.DateTime).ToList();
            if (appointments.Count == 0)
            {
                return 2;
            }

            if ((appointments[0].DateTime - dateFrom).TotalDays > 0 && (appointments[0].DateTime - dateFrom).TotalDays <= 1)
            {
                return 1;
            }
            if (appointments[0].DateTime <= dateFrom)
            {
                return 2;
            }

            return 0;
        }
        
    }
}