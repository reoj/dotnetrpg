using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetrpg.Data;
using dotnetrpg.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace dotnetrpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController:ControllerBase
    {
        #region Readonly Field
        private readonly IAuthRepository _authRepo;
        #endregion

        #region Constructor
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }
        #endregion

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register (UserRegisterDTO request)
        {
            //Populate the response
            var response = await _authRepo.RegisterUser
            (new User {Username = request.Username,} , request.Password);

            // Return Response
            return response.SuccessFlag ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login (UserLoginDTO request)
        {
            //Populate the response
            var response = await _authRepo.Login (request.Username, request.Password);

            // Return Response
            return response.SuccessFlag ? Ok(response) : BadRequest(response);
        }
    }
}