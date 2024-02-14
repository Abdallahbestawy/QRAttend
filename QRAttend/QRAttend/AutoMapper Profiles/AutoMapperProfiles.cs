using AutoMapper;
using QRAttend.Dto;
using QRAttend.Models;

namespace QRAttend.AutoMapper_Profiles
{
    public class AutoMapperProfiles : Profile 
    {
        public AutoMapperProfiles()
        {
            CreateMap<Course, CourseDTO>();
        }
    }
}
