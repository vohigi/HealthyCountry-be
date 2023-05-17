using System.Collections.Generic;

namespace HealthyCountry.RTC.Models;

public class GroupModel
{
    public string Id { get; set; }

    public List<UserModel> Users { get; set; }
}