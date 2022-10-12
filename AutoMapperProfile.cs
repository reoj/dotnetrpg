using AutoMapper;
using dotnetrpg.DTOs.Character;
using dotnetrpg.DTOs.Fights;
using dotnetrpg.DTOs.Skill;
using dotnetrpg.DTOs.Weapon;

namespace dotnetrpg
{
    /// <summary>
    /// Defines the classes that the AutoMapper can map onto each other reducing the need of
    /// explicit asignation of values
    /// </summary>
    public class AutoMapperProfile : Profile
    {
         /// <summary>
        /// Initializes the maps to be Mapped
        /// </summary>
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<AddCharacterDTO,Character>();
            CreateMap<UpdateCharacterDTO, Character>();
            CreateMap<Weapon, GetWeaponDTO>();
            CreateMap<Skill,GetSkillDTO>();
            CreateMap<Character,HighScoreDTO>();
        }
    }
}