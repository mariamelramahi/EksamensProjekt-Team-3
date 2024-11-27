namespace EksamensProjekt.Repos;

public interface IRepo<T>
{
    T GetByID(int id);
    void Create(T entity);
    IEnumerable<T> ReadAll();
    void Update(T entity);
    void Delete(int id);
}
