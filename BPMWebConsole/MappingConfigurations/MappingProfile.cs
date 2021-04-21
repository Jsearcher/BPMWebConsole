using AutoMapper;
using BPMWebConsole.Models.Entities;
using BPMWebConsole.Models.Source;
using BPMWebConsole.Models.ViewModels;

namespace BPMWebConsole.MappingConfigurations
{
    /// <summary>
    /// Entity與ViewModel間的物件對映類別
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public MappingProfile()
        {
            CreateMap<UserRegistrationModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<User, LoginProperty>()
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Email));
        }
    }
}
