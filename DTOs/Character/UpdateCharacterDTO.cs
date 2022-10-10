using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetrpg.DTOs.Character
{
    /// <summary>
    /// Includes the parameters from the Character class that are to be asked from the user
    /// on Character Update
    /// </summary>
    public class UpdateCharacterDTO
    {
         public int Id { get; set; }
        public String Name { get; set; } = "Alpha";
        public int HitPoints { get; set; } = 100;
        public int Strenght { get; set; } = 100;
        public int Intelligence { get; set; } = 100;
        public RpgClass rpgClass { get; set; } = RpgClass.Knight;
    }
}