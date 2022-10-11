using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetrpg.DTOs.Character
{
    public class AddCharacterSkillDTO
    {
        public int CharacterID { get; set; }
        public int SkillID { get; set; }
    }
}