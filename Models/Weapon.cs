namespace dotnetrpg.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Basic Sword";
        public int Damage { get; set; } = 5;
        public Character Holder { get; set; } 
        public int CharacterId { get; set; }
    }
}