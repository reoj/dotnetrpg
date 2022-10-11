using dotnetrpg.DTOs.Character;
using dotnetrpg.DTOs.Weapon;
using dotnetrpg.Services.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnetrpg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;
        #region Constructor
        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }
        #endregion

        #region HTTP Methods
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AddWeapon(AddWeaponDTO nwWeapon)
        {
            var response = await _weaponService.AddWeapon(nwWeapon);
            return response.SuccessFlag == true ? Ok(response) : BadRequest(response);
        }
        #endregion
    }
}