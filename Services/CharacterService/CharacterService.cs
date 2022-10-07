using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using dotnetrpg.DTOs.Character;

namespace dotnetrpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character (),
            new Character {Id = 1, Name = "Beta"}
        };
        private readonly IMapper _mapper;
        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;         
        }
        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newChr)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newChr);
            character.Id = characters.Max(item => item.Id) + 1;
            characters.Add(character);
            serviceResponse.Data = characters.Select
                (item => _mapper.Map<GetCharacterDTO>(item)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            return new ServiceResponse<List<GetCharacterDTO>> 
            {
                Data = characters.Select
                (item => _mapper.Map<GetCharacterDTO>(item)).ToList()
            };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterByID(int id)
        {
            return new ServiceResponse<GetCharacterDTO> 
            {
                Data = _mapper.Map<GetCharacterDTO>
                    (characters.FirstOrDefault(item => item.Id == id)),
            };
        }
    }
}