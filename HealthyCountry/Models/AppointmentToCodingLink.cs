using System;
using MIS.Models;

namespace HealthyCountry.Models
{
    public class AppointmentToCodingLink
    {
        public Guid Id { get; set; }
        public string AppointmentId { get; set; }
        public string CodingId { get; set; }
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