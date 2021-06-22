using System;
using System.Collections.Generic;
using HealthyCountry.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MIS.Models
{
    public class Appointment
    {
        public Appointment(string employeeId, DateTime dateTime, AppointmentStatuses status)
        {
            EmployeeId = employeeId;
            DateTime = dateTime;
            Status = status;
            AppointmentId = Guid.NewGuid().ToString();
        }

        public string AppointmentId { get; set; }
        public string PatientId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime DateTime { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AppointmentStatuses Status { get; set; }
        public string DiagnosisId { get; set; }
        public User Employee { get; set; }
        public User Patient { get; set; }
        public List<AppointmentToReasonLink> Reasons { get; set; } = new List<AppointmentToReasonLink>();
        public List<AppointmentToActionLink> Actions { get; set; } = new List<AppointmentToActionLink>();
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
