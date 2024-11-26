using EksamensProjekt.Models;
using EksamensProjekt.Repos;


namespace EksamensProjekt.Services;


public class AuthLogin
{
    private readonly IUserRepo<User> _userRepo;

    public AuthLogin(IUserRepo<User> userRepo)
    {
        _userRepo = userRepo;
    }


    public bool ValidateLogin(string usernameInput, string passwordInput)
    {
        User user = _userRepo.GetByUsername(usernameInput); // Get user
        return 
            user != null && // Check if user exist
            user.UserPasswordHash == passwordInput; // Check password
    }



}
