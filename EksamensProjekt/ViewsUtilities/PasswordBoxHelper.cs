using System.Windows;
using System.Windows.Controls;

namespace EksamensProjekt.ViewsUtilities
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject d)
        {
            return (string)d.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject d, string value)
        {
            d.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

                if (e.NewValue is string newPassword && passwordBox.Password != newPassword)
                {
                    passwordBox.Password = newPassword;
                }
                else if (string.IsNullOrEmpty(passwordBox.Password) && e.NewValue != null)
                {
                    passwordBox.Password = e.NewValue.ToString();
                }

                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                var currentBindingValue = GetBoundPassword(passwordBox);
                if (passwordBox.Password != currentBindingValue)
                {
                    SetBoundPassword(passwordBox, passwordBox.Password);
                }
            }
        }
    }
}
