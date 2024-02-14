using AutoMapper;
using DoctorFinderHubApi.Dto.DoctorAuth;
using DoctorFinderHubApi.Models;

namespace DoctorFinderHubApi.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<DoctorAuth,DoctorUpdateDto>().ReverseMap();
            CreateMap<DoctorAuth,DeleteDto>().ReverseMap(); 
        }
    }
}
