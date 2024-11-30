using EksamensProjekt.Models;
using EksamensProjekt.Repos;
using EksamensProjekt.Repositories;
using Microsoft.Data.SqlClient;
using System.Windows;
namespace EksamensProjekt.Services;

public class HistoryService
{
    private readonly IRepo<History> historyRepo;

    public HistoryService(IRepo<History> historyRepo)
    {
        this.historyRepo = historyRepo;
    }

    public List<History> GetAllHistories()
    {
        return historyRepo.ReadAll().ToList();
    }
}
