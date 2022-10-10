using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dotnetrpg.Data
{
    /// <summary>
    /// Repository to handle the User Authentication Requests from the AuthController
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        #region Constructor
        public AuthRepository(DataContext context, IConfiguration configuration) // Config injection
        {
            // Entity Framework Data Context
            _context = context;            
            // From appsettings.json
            _configuration = configuration;
        }
        #endregion
        
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            // Prepare service Response
            var response = new ServiceResponse<string>();

            // Search for the Username given in the Database
            var user = await _context.Users.FirstOrDefaultAsync
                (u => u.Username.ToLower() == username.ToLower());

            if (user is null) // Case, Username doesn't exist
            {
                response.SuccessFlag = false;
                response.Message = "Not a valid username";
            }
            else if(!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                // Password and username don't match
                response.SuccessFlag = false;
                response.Message = "Not a valid password";
            }
            else
            {
                // Correct user Login
                response.Data = CreateToken(user);
                response.Message = "Login successful";
            }

            return response;
        }

        public async Task<ServiceResponse<int>> RegisterUser(User user, string password)
        {
            //Preparing necesary objects
            ServiceResponse<int> response = new ServiceResponse<int>();

            //Check if user is not duplicate
            if(await UserExists(user.Username)) 
            {
                response.SuccessFlag = false;
                response.Message = "User already exists";
            } else {
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
    
                //Populating User object with password data
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                // 
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                // 
                response.Data = user.Id;
                response.Message = "User created successfully. User ID returned as Data";        
            }
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            // Returns true if user exists or False if it doesn't
            return await _context.Users.AnyAsync
                (u => u.Username.ToLower() == username.ToLower());
        }

        #region Private Methods
        private void CreatePasswordHash(string password, 
            out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Call to HMAC algorythm
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash
                    (System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // Recreation of the password data using the original salt
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                // Computation of the Hash with the same data as in Creation
                var computedHash = hmac.ComputeHash
                    (System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user){
            string result = String.Empty;

            // Initialize the list of claims for the AuthToken
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            //Generate Symetric Key retrieving a string from appsettings.jason
            SymmetricSecurityKey ssKey = 
                new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes
                    (_configuration.GetSection("AppSettings:Token").Value));

            //Crete Signing credentials
            SigningCredentials credentials = new SigningCredentials
                (ssKey,SecurityAlgorithms.HmacSha512Signature);

            // Create Security Token descriptor
            SecurityTokenDescriptor stDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            // Create the actual JWT security Token
            JwtSecurityTokenHandler jwtstHandler = new JwtSecurityTokenHandler();
            SecurityToken token = jwtstHandler.CreateToken(stDescriptor);

            //Serialization of the token into String
            result = jwtstHandler.WriteToken(token);
            return result;
        }
        #endregion
    }
}