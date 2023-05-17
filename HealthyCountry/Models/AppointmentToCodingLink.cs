using System;

namespace HealthyCountry.Models
{
    public class AppointmentToCodingLink
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid CodingId { get; set; }
        public ICPC2Entity Coding { get; set; }
        public Appointment Appointment { get; set; }
    }

    public class AppointmentToReasonLink : AppointmentToCodingLink
    {
        
    }
    public class AppointmentToActionLink : AppointmentToCodingLink
    {
        
    }
}