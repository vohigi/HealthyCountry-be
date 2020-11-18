using System.Collections.Generic;
using System.Threading.Tasks;
using MIS.Models;

namespace HealthyCountry.Repositories
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetDoctorAppointments(string doctorId);
        Task SaveAppointments(List<Appointment> appointmentsRange);
    }
}