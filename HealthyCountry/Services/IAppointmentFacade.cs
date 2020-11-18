using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MIS.Models;

namespace HealthyCountry.Services
{
    public interface IAppointmentFacade
    {
        Task<List<Appointment>> GetAppointmentsAsync(string doctorId, DateTime dateFrom);
    }
}