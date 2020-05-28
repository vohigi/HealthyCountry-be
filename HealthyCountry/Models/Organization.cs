using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthyCountry.Models
{
  public class Organization
  {
    [Key]
    public string OrganizationId { get; set; }
    public string Name { get; set; }
    public string Edrpou { get; set; }
    public string Address { get; set; }
    public List<User> Employees { get; set; }
    public bool IsActive { get; set; }
  }
}
