namespace EksamensProjekt.Model.Repositories
{
    public interface IRepo<T>
    {
        void Add(T item);
        void Update(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Delete(T item);
    }

}
