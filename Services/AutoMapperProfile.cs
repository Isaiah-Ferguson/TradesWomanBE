using TradesWomanBE.Models;
using AutoMapper;

namespace TradesWomanBE.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ClientModel, ClientModel>();
            CreateMap<MeetingsModel, MeetingsModel>();
            CreateMap<ProgramModel, ProgramModel>();
            CreateMap<RecruiterModel, RecruiterModel>();
            CreateMap<StipendsModel, StipendsModel>();
        }
    }
}