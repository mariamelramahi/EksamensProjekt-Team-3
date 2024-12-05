using System.Windows;

namespace EksamensProjekt.Services.Navigation;

public interface INavigationService
{ 
    void RegisterFactory<TView>(Func<TView> createView) where TView : Window;
    void NavigateTo<TView>() where TView : Window;
    // OLD void NavigateTo<T>() where T : Window, new();
    void CloseCurrentWindow();
}
