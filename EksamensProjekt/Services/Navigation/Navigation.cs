using System.Windows;

namespace EksamensProjekt.Services.Navigation;

public class NavigationService : INavigationService
{
    public void NavigateTo<T>() where T : Window, new()
    {
        // Create a new instance of the specified window type
        T window = new T();

        // Close the current window
        CloseCurrentWindow();

        // Show the new window
        window.Show();
    }

    public void CloseCurrentWindow()
    {
        var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
        currentWindow?.Close();
    }
}
