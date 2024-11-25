namespace EksamensProjekt.Models.Repositories;

public interface IUserRepo<User>
{
    User GetByUsername(string userName);
}
