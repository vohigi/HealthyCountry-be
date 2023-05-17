using System;
using System.Collections.Generic;

namespace HealthyCountry.Models
{
  public class Organization
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Edrpou { get; set; }
    public string Address { get; set; }
    public List<User> Employees { get; set; }
    public bool IsActive { get; set; }
  }
}
