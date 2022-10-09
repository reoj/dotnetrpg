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

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> response = 
            new ServiceResponse<List<GetCharacterDTO>>();

            try {
                Character character = characters.First(item => item.Id == id);
                characters.Remove(character);
                response.Data = characters.Select(item => _mapper.Map<GetCharacterDTO>(item)).ToList();
                return response;
            } catch (Exception ex) {
                response.SuccessFlag = false;
                response.Message = ex.Message.ToString();
                return response;
            }
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
            ServiceResponse<GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();
            try {
                var character = _mapper.Map<GetCharacterDTO>
                        (characters.FirstOrDefault(item => item.Id == id));
                if (character != null)
                {
                    response.Data = character;
                    response.Message = "Character returned correctly";
                    return  response;                    
                }else{
                    throw new NullReferenceException();
                }
            } catch (Exception ex) {
                response.SuccessFlag = false;
                response.Message = ex.Message.ToString();
                return response;
            }
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter
        (UpdateCharacterDTO updatedCharacter)
        {
            ServiceResponse<GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();

            try {
                var character = characters.FirstOrDefault(item => item.Id == updatedCharacter.Id);                
    
                if(character != null) {
                    character = _mapper.Map<Character>(updatedCharacter);
                    /*character.Name = updatedCharacter.Name;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Intelligence = updatedCharacter.Intelligence;
                    character.Strenght = updatedCharacter.Strenght;
                    character.rpgClass = updatedCharacter.rpgClass;*/
                    response.Data = _mapper.Map<GetCharacterDTO>(character); 
                    response.Message = "Update Successful";
                } else {
                    throw new NullReferenceException("No character exists for given ID");
                }    
                
            } catch (Exception ex) {
                response.SuccessFlag = false;
                response.Message = ex.Message.ToString();
            }
            return response;
        }
    }
}