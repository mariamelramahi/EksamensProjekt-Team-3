using System.Windows;

namespace EksamensProjekt.Services.Navigation;

public class NavigationService : INavigationService
{
    public void CloseCurrentWindow()
    {
        var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
        currentWindow?.Close();
    }

    private readonly Dictionary<Type, Func<Window>> _factories = new();
    public void RegisterFactory<TView>(Func<TView> createView) where TView : Window
    {
        _factories[typeof(TView)] = createView;
    }
    public void NavigateTo<TView>(Func<TView> createView) where TView : Window
    {
        throw new NotImplementedException();
    }
    
    public void NavigateTo<TView>() where TView : Window
    {
        if (_factories.TryGetValue(typeof(TView), out var factory))
        {
            var newWindow = factory();
            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            currentWindow?.Close();
            newWindow.Show();
        }
        else
        {
            throw new InvalidOperationException($"No factory registered for type {typeof(TView).Name}");
        }
    }
    public void NavigateToWithViewModel<TView, TViewModel>(Action<TViewModel> configureViewModel)
       where TView : Window
       where TViewModel : class
    {
        if (_factories.TryGetValue(typeof(TView), out var factory))
        {
            var newWindow = factory();
            if (newWindow.DataContext is TViewModel viewModel)
            {
                configureViewModel(viewModel);
            }

            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            currentWindow?.Close();
            newWindow.Show();
        }
        else
        {
            throw new InvalidOperationException($"No factory registered for type {typeof(TView).Name}");
        }
    }
}
