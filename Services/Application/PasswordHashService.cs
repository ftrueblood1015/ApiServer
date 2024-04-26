using Domain.Interfaces.Services.Application;
using System.Security.Cryptography;
using System.Text;

namespace Services.Application
{
    public class PasswordHashService : IPasswordHashService
    {
        public PasswordHashService()
        {
            
        }

        public string CreateHash(string plainTextPassword)
        {
            if (plainTextPassword == null || plainTextPassword == String.Empty) 
            { 
                throw new ArgumentNullException(nameof(plainTextPassword));
            }

            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash 
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));

                // Return hashed string   
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            return CreateHash(plainTextPassword) == hashedPassword;
        }
    }
}
