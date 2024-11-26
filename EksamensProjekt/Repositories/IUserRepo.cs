namespace EksamensProjekt.Repos;

public interface IUserRepo<User>
{
    User GetByUsername(string userName);
}
