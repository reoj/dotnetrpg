using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnetrpg.Data
{
    /// <summary>
    /// Repository to handle the User Authentication Requests from the AuthController
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        #region Constructor
        public AuthRepository(DataContext context)
        {
            this._context = context;            
        }
        #endregion
        
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync
                (u => u.Username.ToLower() == username.ToLower());
            if (user is null)
            {
                response.SuccessFlag = false;
                response.Message = "Not a valid username";
            }
            else if(!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                response.SuccessFlag = false;
                response.Message = "Not a valid password";
            }
            else
            {
                response.Data = user.Id.ToString();
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
    
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                
                response.Data = user.Id;
                response.Message = "User created successfully. User ID returned as Data";        
            }
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync
                (u => u.Username.ToLower() == username.ToLower());
        }

        #region Private Methods
        private void CreatePasswordHash(string password, 
            out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash
                    (System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash
                    (System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        #endregion
    }
}