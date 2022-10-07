using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetrpg.DTOs.Character;

namespace dotnetrpg.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters();
        Task<ServiceResponse<GetCharacterDTO>> GetCharacterByID(int id);
        Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newChr);
    }
}