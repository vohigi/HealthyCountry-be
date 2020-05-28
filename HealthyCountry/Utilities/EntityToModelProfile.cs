using AutoMapper;
using HealthyCountry.DataModels;
using HealthyCountry.Models;

namespace HealthyCountry.Utilities
{
    public class EntityToModelProfile:Profile
    {
        public EntityToModelProfile()
        {
            CreateMap<UserRequestDataModel, User>().ReverseMap();
        }
    }
}