using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using dotnetrpg.Data;
using dotnetrpg.DTOs.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnetrpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        public DataContext _context;
        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;         
        }
        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newChr)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newChr);
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters.Select
                (item => _mapper.Map<GetCharacterDTO>(item)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> response = 
            new ServiceResponse<List<GetCharacterDTO>>();

            try {
                var dbCharacter = await _context.Characters.FirstAsync(item => item.Id == id);
                _context.Remove(dbCharacter);

                await _context.SaveChangesAsync();
                response.Data = _context.Characters.Select
                    (item => _mapper.Map<GetCharacterDTO>(item)).ToList();
                return response;
            } catch (Exception ex) {
                response.SuccessFlag = false;
                response.Message = ex.Message.ToString();
                return response;
            }
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDTO>>();
            try {
                var dbCharacters = await _context.Characters.ToListAsync();
                response.Data = dbCharacters.Select
                    (item => _mapper.Map<GetCharacterDTO>(item)).ToList();
                    response.Message = "Database Query executed successfully";
            } catch (Exception err) {
                response.Message = err.Message.ToString();
                response.SuccessFlag = false;
            }            
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterByID(int id)
        {
            ServiceResponse<GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();
            try {
                var dbCharacter = 
                    await _context.Characters.FirstOrDefaultAsync(item => item.Id == id);
                if (dbCharacter != null)
                {
                    response.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
                }else{
                    throw new NullReferenceException();
                }
            } catch (Exception ex) {
                response.SuccessFlag = false;
                response.Message = ex.Message.ToString();
            }
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter
        (UpdateCharacterDTO updatedCharacter)
        {
            ServiceResponse<GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();

            try {
                var character = await _context.Characters.FirstOrDefaultAsync
                    (item => item.Id == updatedCharacter.Id);                
    
                if(character != null) {
                    character = _mapper.Map<Character>(updatedCharacter);
                    /*character.Name = updatedCharacter.Name;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Intelligence = updatedCharacter.Intelligence;
                    character.Strenght = updatedCharacter.Strenght;
                    character.rpgClass = updatedCharacter.rpgClass;*/

                    await _context.SaveChangesAsync();

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