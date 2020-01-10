using AutoMapper;
using Template.Core.Models.Dtos;
using Template.Data.Entities.Identity;

namespace Template.Core.Profiles
{
    public partial class AutoMapperProfile : Profile
    {
        private void MapUser()
        {
            this.CreateMap<User, UserDto>()
                .ReverseMap();

            this.CreateMap<CredentialsDto, User>()
                .ForMember(
                    entity => entity.Email,
                    map => map.MapFrom(dto => dto.UserName));
        }
    }
}