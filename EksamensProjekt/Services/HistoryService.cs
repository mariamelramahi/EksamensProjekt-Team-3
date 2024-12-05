using EksamensProjekt.Models;
using EksamensProjekt.Repositories;

namespace EksamensProjekt.Services;

public class HistoryService
{
    private readonly IHistoryRepo historyRepo;

    public HistoryService(IHistoryRepo historyRepo)
    {
        this.historyRepo = historyRepo;
    }

    public List<History> GetAllHistories()
    {
        return historyRepo.ReadAll().ToList();
    }
}
