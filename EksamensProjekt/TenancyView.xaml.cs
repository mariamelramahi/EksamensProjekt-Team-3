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
        public ObservableCollection<Lejemål> LejemålList { get; set; }
        public ObservableCollection<string> TenantList { get; set; }
        public ObservableCollection<string> TenancyStatusList { get; set; } = new ObservableCollection<string> { "udlejet", "lejet", "Under Renovering" };
        public ObservableCollection<string> DogAllowedList { get; set; } = ["ja", "nej"];
        public TenancyView()
        {
            InitializeComponent();
            LejemålList = new ObservableCollection<Lejemål>
            {
                new Lejemål { Address = "Skovløbervej 11, 2400 København NV", Rooms = 3, Size = 85, Tenant = "Anders Jensen", Company = "Boligselskabet hej hej", Owner = "Maryanne" },
                new Lejemål { Address = "Bogtrykkervej 32, 2400 København NV", Rooms = 2, Size = 65, Tenant = "Sofie Hansen" },
                new Lejemål { Address = "Vollsmosse 13H, 5240 Odense C", Rooms = 4, Size = 100, Tenant = "Thomas Nielsen" },
                new Lejemål { Address = "Hybovej 5A, 9440 Aabybro", Rooms = 5, Size = 120, Tenant = "Marie Petersen" }
            };
            TenantList = new ObservableCollection<string>
            {
                "Anders Jensen",
                "Sofie Hansen",
                "Thomas Nielsen",
                "Marie Petersen",
                "Frederik Larsen",
                "Camilla Sørensen"
            };

            DataContext = this;
            AddressListBox.ItemsSource = LejemålList;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ændringer er gemt!");
            // Her kunne du evt. implementere yderligere logik for at opdatere lejemålsdataene permanent (fx i en database)
        }

        private void NewTenant_Click(object sender, RoutedEventArgs e)
        {
            AddTenantView _AddTenantView = new AddTenantView();
            _AddTenantView.Show();
        }

       
    }
    public class Lejemål
    {
        public string Address { get; set; }
        public int TenancyID { get; set; }
        public string TenancyStatus { get; set; } // udlejet, lejet, opsagt
        public DateTime? MoveInDate { get; set; }
        public DateTime? MoveOutDate { get; set; }
        public int Size { get; set; }
        public decimal Rent { get; set; }
        public int Rooms { get; set; }
        public int Bathrooms { get; set; }
        public string DogAllowed { get; set; } // ja, nej
        public string Company { get; set; }
        public string Tenant { get; set; }

        public string Owner { get; set; }
    }


}
