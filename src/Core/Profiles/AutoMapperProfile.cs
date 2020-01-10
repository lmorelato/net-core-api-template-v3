using System;

using AutoMapper;
using Template.Core.Models.Dtos.Bases;
using Template.Data.Entities.Interfaces;

namespace Template.Core.Profiles
{
    public partial class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.MapUser();
        }

        private void MapDtoToEntity(BaseDto dto, IBaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}