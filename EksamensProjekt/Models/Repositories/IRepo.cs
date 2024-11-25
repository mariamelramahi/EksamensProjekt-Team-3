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
        void Create(T entity);
        IEnumerable<T> ReadAll();
        void Update(T entity);
        void Delete(T entity);
    }
}
