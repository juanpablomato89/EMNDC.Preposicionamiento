using AutoMapper;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Responses;

namespace EMNDC.Preposicionamiento.Utils
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserModel, BasicUserResponse>()
                .ForMember(d => d.Id, opts => opts.MapFrom(source => source.Id))
                .ForMember(d => d.Name, opts => opts.MapFrom(source => source.Name))
                .ForMember(d => d.LastName, opts => opts.MapFrom(source => source.LastName))
                .ForMember(d => d.Email, opts => opts.MapFrom(source => source.Email));

            CreateMap<ActiveDirectoryUserResponse, UserModel>()
                .ForMember(d => d.Name, opts => opts.MapFrom(source => source.FirstName))
                .ForMember(d => d.LastName, opts => opts.MapFrom(source => source.LastName))
                .ForMember(d => d.IsUserDomain, opts => opts.MapFrom(source => source.IsEnabled))
                .ForMember(d => d.Email, opts => opts.MapFrom(source => source.Email));
        }
    }
}