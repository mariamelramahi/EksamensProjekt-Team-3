using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EksamensProjekt.Utilities;

namespace EksamensProjekt
{
    /// <summary>
    /// Interaction logic for TenancyView.xaml
    /// </summary>
    public partial class TenancyView : Window
    {

        TenancyViewModel tvm = new TenancyViewModel(App.Configuration);
        public TenancyView()
        {
            InitializeComponent();
            this.DataContext = tvm;
        }


    }
    


}
