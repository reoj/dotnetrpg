using System.Text.Json.Serialization;

namespace dotnetrpg.Models
{
    /// <summary>
    /// Enum to represent the basic clases that an RPG Character can have
    /// </summary>
    [JsonConverter (typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Knight = 1,
        Mage = 2,
        Cleric = 3, 
    }
}