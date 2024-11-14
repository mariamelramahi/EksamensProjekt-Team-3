using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksamensProjekt.Models.Repositories
{
    public interface IRepo<T>
    {
        T GetByID(int id);
        T GetByUsername(string userName);
        void Create(T entity);
        IEnumerable<T> ReadAll();
        void Update(T entity);
        void Delete(int entity);
    }
}
