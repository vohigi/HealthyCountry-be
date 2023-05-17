using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HealthyCountry.Models
{
    public class Appointment
    {
        public Appointment(Guid employeeId, DateTime dateTime, AppointmentStatuses status)
        {
            EmployeeId = employeeId;
            DateTime = dateTime;
            Status = status;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid? PatientId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime DateTime { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AppointmentStatuses Status { get; set; }
        public Guid? DiagnosisId { get; set; }
        public User Employee { get; set; }
        public User Patient { get; set; }
        public List<AppointmentToReasonLink> Reasons { get; set; } = new();
        public List<AppointmentToActionLink> Actions { get; set; } = new();
        public DiagnosisEntity Diagnosis { get; set; }
        
        public string Comment { get; set; }
    }

    public enum AppointmentStatuses
    {
        FREE,
        BOOKED,
        INPROGRESS,
        FINISHED,
        CANCELED
    }
    
}
