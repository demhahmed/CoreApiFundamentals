using AutoMapper;
using CoreCodeCamp.Models;
using Microsoft.Extensions.Options;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            CreateMap<Camp, CampModel>().ReverseMap();

            CreateMap<Talk, TalkModel>()
                .ReverseMap()
                .ForMember(t => t.Camp, opt => opt.Ignore())
                .ForMember(t => t.Speaker, opt => opt.Ignore());

            CreateMap<Speaker, SpeakerModel>().ReverseMap();

        }
    }
}