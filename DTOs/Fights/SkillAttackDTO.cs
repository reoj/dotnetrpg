using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetrpg.DTOs.Fights
{
    public class SkillAttackDTO
    {
        public int AttackerID { get; set; }
        public int OponentID { get; set; }
        public int SkillId { get; set; }
    }
}