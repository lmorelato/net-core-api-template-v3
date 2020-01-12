using AutoMapper;

namespace Template.Core.Profiles
{
    public sealed partial class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.MapUser();
        }
    }
}