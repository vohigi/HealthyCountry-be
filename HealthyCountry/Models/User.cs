using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HealthyCountry.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string TaxId { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public UserRoles Role { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DoctorSpecializations Specialization { get; set; }
        public bool IsActive { get; set; }
        public Guid? OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public List<Appointment> AppointmentsAsDoctor { get; set; } = new List<Appointment>();
        public List<Appointment> AppointmentsAsPatient { get; set; } = new List<Appointment>();
    }

    public enum UserRoles
    {
        DOCTOR,
        OWNER,
        PATIENT,
        ADMIN
    }
    public enum DoctorSpecializations
    {
        Therapist = 0,
        Pediatrician = 1
    }
};
