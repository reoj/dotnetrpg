using dotnetrpg.DTOs.Fights;
using dotnetrpg.Services.FightService;
using Microsoft.AspNetCore.Mvc;

namespace dotnetrpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FightsController : ControllerBase
    {
        private readonly IFightService _fService;
        #region Constructor
        public FightsController(IFightService fService)
        {
            _fService = fService;
        }
        #endregion

        #region HTTP Methods
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ResultAttackDTO>>> WeaponAttack(WeaponAttackDTO attack)
        {
            var response = await _fService.WeaponAttack(attack);
            return response.SuccessFlag ? Ok(response) : BadRequest(response);
        }
        #endregion
    }
}