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

        #endregion
    }
}