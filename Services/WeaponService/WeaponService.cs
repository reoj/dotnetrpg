using System.Security.Claims;
using AutoMapper;
using dotnetrpg.Data;
using dotnetrpg.DTOs.Character;
using dotnetrpg.DTOs.Weapon;
using Microsoft.EntityFrameworkCore;

namespace dotnetrpg.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        
        #region Fields
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpAccess;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public WeaponService(DataContext context, IHttpContextAccessor httpAccess ,IMapper mapper )
        {
            _context = context;
            _httpAccess = httpAccess;
            _mapper = mapper;
        }
        #endregion

        #region Public Methods
        public async Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDTO nwWeapon)
        {
            // Prepare Response
            ServiceResponse <GetCharacterDTO> response = new ServiceResponse<GetCharacterDTO>();
            
            // Attempt to generate the weapon
            try
            {
                if(_httpAccess.HttpContext != null)
                {
                    int fromHTTPAcc = int.Parse(_httpAccess.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                    var character =await _context.Characters
                        .FirstOrDefaultAsync(c => c.userOwner != null
                            && c.Id == nwWeapon.CharacterId
                            && c.userOwner.Id == fromHTTPAcc);                    
                    if (character != null)
                    {
                        Weapon weapon = new Weapon{
                            Name = nwWeapon.Name,
                            Damage = nwWeapon.Damage,
                            Holder = character
                        };

                        _context.Weapons.Add(weapon);
                        await _context.SaveChangesAsync();

                        response.SuccessFlag = true;
                        response.Message = "Operation concluded correctly";
                    }
                    else
                    {
                        throw new NullReferenceException("No Character was found to bear that Weapon");
                    }                  
                    
                }
            }
            catch (Exception err)
            {
                
                response.SuccessFlag = false;
                response.Message = err.Message;
            }
            return response;
        }
        #endregion
    }
}