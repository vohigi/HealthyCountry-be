using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MIS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HealthyCountry.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }
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
        public bool IsActive { get; set; }
        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public List<Appointment> AppointmentsAsDoctor { get; set; }
        public List<Appointment> AppointmentsAsPatient { get; set; }
    }

    public enum UserRoles
    {
        DOCTOR,
        OWNER,
        PATIENT,
        ADMIN
    }
};
