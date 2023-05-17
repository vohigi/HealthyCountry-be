using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyCountry.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthyCountry.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Appointment> GetDoctorAppointments(Guid doctorId)
        {
            return _dbContext.Appointments.Include(x => x.Employee)
                .Where(x => x.EmployeeId == doctorId)
                .OrderByDescending(x => x.DateTime).ToList();
        }

        public async Task SaveAppointments(List<Appointment> appointmentsRange)
        {
            await _dbContext.Appointments.AddRangeAsync(appointmentsRange);
            await _dbContext.SaveChangesAsync();
        }
    }
}