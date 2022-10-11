using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetrpg.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int Damage { get; set; }
        public Character Holder { get; set; } = new Character();
        public int CharacterId { get; set; }
    }
}