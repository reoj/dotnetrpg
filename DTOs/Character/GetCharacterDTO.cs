using dotnetrpg.DTOs.Skill;
using dotnetrpg.DTOs.Weapon;

namespace dotnetrpg.DTOs.Character
{
    /// <summary>
    /// Includes the parameters from the Character class that are to be shown to the user
    /// on Request
    /// </summary>
    public class GetCharacterDTO
    {
        public int Id { get; set; }
        public String Name { get; set; } = "Alpha";
        public int HitPoints { get; set; } = 100;
        public int Strenght { get; set; } = 100;
        public int Intelligence { get; set; } = 100;
        public RpgClass rpgClass { get; set; } = RpgClass.Knight;
        public GetWeaponDTO CurrentWeapon { get; set; } 
        public List<GetSkillDTO> Skills { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}