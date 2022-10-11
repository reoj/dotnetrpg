using System.Security.Claims;
using AutoMapper;
using dotnetrpg.Data;
using dotnetrpg.DTOs.Character;
using Microsoft.EntityFrameworkCore;

namespace dotnetrpg.Services.CharacterService
{
    /// <summary>
    /// Handles the basic calls from the Character Controller
    /// </summary>
    public class CharacterService : ICharacterService
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        public DataContext _context;
        #endregion

        #region Constructor
        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;      
        }
        #endregion

        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newChr)
        {
            // Preparing necesary objects
            var serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = _mapper.Map<Character>(newChr);

            // Add foregin key to the object
            character.userOwner = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserID());

            //Request the object to be added to the Database
            _context.Characters.Add(character);

            // Save changes in the DB
            await _context.SaveChangesAsync();

            // Populate Server response
            serviceResponse.Data = await _context.Characters
                .Where(item => item.userOwner != null && item.userOwner.Id == GetUserID())
                .Select(item => _mapper.Map<GetCharacterDTO>(item))
                .ToListAsync();            
            serviceResponse.Message = "Character registered correctly";


            // Informs
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            // Prepare the Response object to be returned
            ServiceResponse<List<GetCharacterDTO>> response = 
            new ServiceResponse<List<GetCharacterDTO>>();

            // Attempt to find the character to ba deleted from the DataBase
            try {
                //Locate the register in the DB
                var dbCharacter = await _context.Characters
                    .FirstOrDefaultAsync(item => item.Id == id && item.userOwner != null && item.userOwner.Id == GetUserID());

                if (dbCharacter != null)
                {
                    //Request removal to the DB
                    _context.Remove(dbCharacter);
                    //Request DB to save changes.
                    await _context.SaveChangesAsync();

                    //Return the list of remaining Characters in DB
                    response.Data = _context.Characters
                        .Where(ch => ch.userOwner != null && ch.userOwner.Id == GetUserID())
                        .Select(item => _mapper.Map<GetCharacterDTO>(item)).ToList();
                    
                    //Inform the user of successfull operation
                    response.Message = "Character deleted correctly";
                }else{
                    response.SuccessFlag = false;
                    response.Message = "Character not found";
                }
                
                return response;

            } catch (Exception ex) {
                // Character object could not be fetched from DB
                response.SuccessFlag = false;

                //The response includes the Message from the exception
                response.Message = ex.Message.ToString();
                return response;
            }
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDTO>>();

            // Attempt to get Character information from the DB
            try {
                var dbCharacters = await _context.Characters
                    .Where(ch => ch.userOwner != null && ch.userOwner.Id == GetUserID())
                    .ToListAsync();

                response.Data = dbCharacters.Select
                    (item => _mapper.Map<GetCharacterDTO>(item)).ToList();

                // Information on changes
                response.Message = "Database Query executed successfully";
            } 
            catch (Exception err) // Information could not be fetched
            {
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
                    await _context.Characters
                        .FirstOrDefaultAsync(item => item.Id == id && item.Id == GetUserID());
                if (dbCharacter != null)
                {
                    response.Data = _mapper.Map<GetCharacterDTO>(dbCharacter);
                }else{
                    throw new NullReferenceException();
                }
            } catch (Exception ex) {
                response.SuccessFlag = false;
                response.Message = "Invlaid Request: This character doesn't exist or is not owned by the User: " 
                    + ex.Message.ToString();
            }
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter
        (UpdateCharacterDTO updatedCharacter)
        {
            ServiceResponse<GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();

            try {
                var character = await _context.Characters
                    .Include(ch => ch.userOwner)
                    .FirstOrDefaultAsync(item => item.Id == updatedCharacter.Id);               
    
                if(character != null && character.userOwner != null && character.userOwner.Id == GetUserID()) 
                {
                    character = _mapper.Map<Character>(updatedCharacter);
                    // This is what AutoMapper does
                    /*character.Name = updatedCharacter.Name;
                    character.HitPoints = updatedCharacter.HitPoints;
                    character.Intelligence = updatedCharacter.Intelligence;
                    character.Strenght = updatedCharacter.Strenght;
                    character.rpgClass = updatedCharacter.rpgClass;*/

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetCharacterDTO>(character); 
                    response.Message = "Update Successful";
                } else {
                    response.Message = "character not found";
                    response.SuccessFlag = false;
                }    
                
            } catch (Exception ex) { // Update Failed
                response.SuccessFlag = false;
                response.Message = ex.Message.ToString();
            }
            return response;
        }
        
        #region Private Methods
        private int GetUserID()
        {
            if (_httpContext.HttpContext != null)
            {
                return int.Parse(_httpContext.HttpContext.User
                    .FindFirstValue(ClaimTypes.NameIdentifier));
            }else{
                throw new NullReferenceException();
            }
        }
        #endregion
    }
}