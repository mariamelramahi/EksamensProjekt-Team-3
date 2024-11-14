using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EksamensProjekt
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window

    {
        LoginViewModel lvm = new LoginViewModel(App.Configuration);
        public LoginView()
        {
            InitializeComponent();
            this.DataContext = lvm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TenancyView _TenancyView = new();
            this.Close();
            _TenancyView.Show();
        }
    }
}