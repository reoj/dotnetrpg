using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetrpg.DTOs.User
{
    /// <summary>
    /// Includes the parameters from the User class that are to be sent in Registration
    /// </summary>
    public class UserRegisterDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}