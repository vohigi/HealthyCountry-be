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
        public string ReasonId { get; set; }
        //public List<string> ActionIds { get; set; }
        public string DiagnosisId { get; set; }
        public User Employee { get; set; }
        public User Patient { get; set; }
        public ICPC2Entity Reason { get; set; }
        //public List<ICPC2Entity> Action { get; set; }
        public ICPC2Entity Diagnosis { get; set; }
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
