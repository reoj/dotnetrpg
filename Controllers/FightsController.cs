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
        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<ResultAttackDTO>>> WeaponAttack(WeaponAttackDTO attack)
        {
            var response = await _fService.WeaponAttack(attack);
            return response.SuccessFlag ? Ok(response) : BadRequest(response);
        }
        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<ResultAttackDTO>>> SkillAttack(SkillAttackDTO attack)
        {
            var response = await _fService.SkillAttack(attack);
            return response.SuccessFlag ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ResultAttackDTO>>> Fight (FightRequestDTO fightRequest)
        {
            var response = await _fService.Fight(fightRequest);
            return response.SuccessFlag ? Ok(response) : BadRequest(response);
        }

        [HttpPost("High Scores")]
        public async Task<ActionResult<ServiceResponse<List<HighScoreDTO>>>> GetHighScores ()
        {
            var response = await _fService.GetHighScores();
            return response.SuccessFlag ? Ok(response) : BadRequest(response);
        }
        #endregion
    }
}