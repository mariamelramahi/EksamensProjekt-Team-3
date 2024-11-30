using EksamensProjekt.ViewModels;
using System.Windows;

namespace EksamensProjekt
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : Window
    {
        public HistoryView(HistoryViewModel Hvm)
        {
            InitializeComponent();
            this.DataContext = Hvm;
        }
    }
}
