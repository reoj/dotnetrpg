using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetrpg.DTOs.Character
{
    public class UpdateCharacterDTO
    {
         public int Id { get; set; }
        public String Name { get; set; } = "Alpha";
        public int HitPoints { get; set; } = 100;
        public int Strenght { get; set; } = 10;
        public int Intelligence { get; set; }
        public RpgClass rpgClass { get; set; } = RpgClass.Knight;
    }
}