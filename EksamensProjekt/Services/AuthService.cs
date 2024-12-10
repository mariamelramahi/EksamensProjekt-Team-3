using EksamensProjekt.Models;
using EksamensProjekt.Repos;
using System.Security.Cryptography;
using System.Text;

namespace EksamensProjekt.Services
{

    public class AuthLogin
    {
        private readonly IUserRepo<User> _userRepo;

        public AuthLogin(IUserRepo<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public bool ValidateLogin(string usernameInput, string passwordInput)
        {
            User user = _userRepo.GetByUsername(usernameInput);

            if (user == null)
                return false;

            // Validate with hash
            return VerifyPasswordHash(passwordInput, user.UserPasswordHash);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "");
                return hashString == storedHash;
            }
        }

    }
}