using System.Windows;

namespace EksamensProjekt.Services.Navigation;

public interface INavigationService
{ 
    void RegisterFactory<TView>(Func<TView> createView) where TView : Window;
    void NavigateTo<TView>() where TView : Window;

    void NavigateToWithViewModel<TView, TViewModel>(Action<TViewModel> configureViewModel)
       where TView : Window
       where TViewModel : class;


    void CloseCurrentWindow();
}
