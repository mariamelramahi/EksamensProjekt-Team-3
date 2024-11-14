
using EksamensProjekt.Models;
using EksamensProjekt.Models.Repositories;

namespace EksamensProjekt.Services;


public class AuthLogin
{
    private readonly IRepo<User> _userRepo;

    public AuthLogin(IRepo<User> userRepo)
    {
        _userRepo = userRepo;
    }


    public bool ValidateLogin(string usernameInput, string passwordInput)
    {
        User user = _userRepo.GetByUsername(usernameInput); // Get user
        return 
            user != null && // Check if user exist
            user.PasswordHash == passwordInput; // Check password
    }



}
