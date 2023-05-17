using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyCountry.Models;

namespace HealthyCountry.Repositories
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetDoctorAppointments(Guid doctorId);
        Task SaveAppointments(List<Appointment> appointmentsRange);
    }
}