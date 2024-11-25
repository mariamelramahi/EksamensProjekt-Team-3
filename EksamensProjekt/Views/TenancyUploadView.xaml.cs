using System;
using System.Collections.Generic;
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
using System.Windows;
using EksamensProjekt.ViewModels;

namespace EksamensProjekt;

/// <summary>
/// Interaction logic for TenancyUpload.xaml
/// </summary>
public partial class TenancyUploadView : Window
{
    public TenancyUploadView(TenancyUploadViewModel tvum)
    {
        InitializeComponent();
        DataContext = tvum;
    }
}
