using System.Windows;

namespace EksamensProjekt.Services.Navigation;

public interface INavigationService
{
    void NavigateTo<T>() where T : Window, new();
    void CloseCurrentWindow();
}
