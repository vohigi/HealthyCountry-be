using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyCountry.Models;

namespace HealthyCountry.Services
{
    public interface IAppointmentFacade
    {
        Task<List<Appointment>> GetAppointmentsAsync(string doctorId, DateTime dateFrom);
    }
}