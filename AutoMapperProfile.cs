using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnetrpg.DTOs.Character;

namespace dotnetrpg
{
    /// <summary>
    /// Defines the classes that the AutoMapper can map onto each other reducing the need of
    /// explicit asignation of values
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<AddCharacterDTO,Character>();
            CreateMap<UpdateCharacterDTO, Character>();
        }
    }
}