using System.Collections.Generic;
using System.Security.Claims;
using dotnetrpg.DTOs.Character;
using dotnetrpg.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetrpg.Models
{
    /// <summary>
    /// Controller for the in-game characters
    /// </summary>
    [Authorize]                 //Ask for Authorization
    [ApiController]             //Mark as Controller
    [Route("api/[controller]")] // Route
    public class CharacterController : ControllerBase
    {
        /// <summary>
        /// Instance of the Characters Service
        /// </summary>
        private readonly ICharacterService _characterService;

        #region HTTP Methods
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;            
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> Get()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetSingle(int id)
        {
            var response = await _characterService.GetCharacterByID(id);
            return response.Data != null ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> AddCharacter
        (AddCharacterDTO newChr)
        {
            return Ok(await _characterService.AddCharacter(newChr));
        }
        
        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> AddSkill(AddCharacterSkillDTO nwChSkill)
        {
            var response = await _characterService.AddCharacterSkill(nwChSkill);
            return response.SuccessFlag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> UpdateCharacter
        (UpdateCharacterDTO updatedCharacter)
        {
            var response = await _characterService.UpdateCharacter(updatedCharacter);
            return response.Data is null ? NotFound(response): Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> DeleteCharacter
        (int id)
        {
            var response = await _characterService.DeleteCharacter(id);
            if (response.Data != null)
            {
                return Ok(response);
            }else{
                return NotFound(response);
            } 
        }
        #endregion
    }

}