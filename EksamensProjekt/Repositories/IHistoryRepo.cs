using EksamensProjekt.Models;

namespace EksamensProjekt.Repositories;

public interface IHistoryRepo
{
    IEnumerable<History> ReadAll();
}
