using HealthyCountry.Repositories;

namespace HealthyCountry.Services
{
    public class GetAppointmentsFacadeSingleton
    {
        private IAppointmentFacade _appointmentFacade;

        public IAppointmentFacade GetFacade(ApplicationDbContext dbContext)
        {
            if (_appointmentFacade == null)
            {
                _appointmentFacade = new AppointmentService(dbContext);
                return _appointmentFacade;
            }

            return _appointmentFacade;
        }
    }
}