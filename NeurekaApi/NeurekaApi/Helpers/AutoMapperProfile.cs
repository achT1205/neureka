using System;
using NeurekaApi.Dtos;
using NeurekaDAL.Models;

namespace NeurekaApi.Helpers
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Visit, Event>()
                .ForMember(e => e.Id, opt => opt.MapFrom(v => v.Id))
                .ForMember(e => e.PatientId, opt => opt.MapFrom(v => v.PatientId))
                .ForMember(e => e.Color, opt => opt.MapFrom(v => v.Color))
                .ForMember(e => e.Name, opt => opt.MapFrom(v => v.Title))
                .ForMember(e => e.Start, opt => opt.MapFrom(v => $"{v.StartDate} {v.StartTime}"))
                .ForMember(e => e.End, opt => opt.MapFrom(v => $"{v.EndDate} {v.EndTime}"));
            CreateMap<Visit, VisitDto>();
            CreateMap<VisitDto, Visit>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

        }
    }
}
