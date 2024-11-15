﻿using System.Windows;

namespace EksamensProjekt.Services.Navigation;

public class NavigationService : INavigationService
{

    //public void NavigateToTwo<T>(Func<T> Tview) where T : Window
    //{
    //    // Create the window using the factory method
    //    T window = createView();

    //    // Close the current window (optional)
    //    var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault();
    //    currentWindow?.Close();

    //    // Show the new window
    //    window.Show();
    //}


    //public void NavigateTo<T>() where T : Window, new()
    //{
    //    // Create a new instance of the specified window type
    //    T window = new T();

    //    // Close the current window
    //    CloseCurrentWindow();

    //    // Show the new window
    //    window.Show();
    //}

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
}
