using FinanceApp.Scenes.AdminPages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinanceApp.Scenes
{

    public partial class NavBarAdmin : UserControl
    {
        public NavBarAdmin()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            NavBarAdmin navBarAdmin = new();
            NavBar navBar = new();

            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            Grid mainGrid = mainWindow.MainGrid;

            if (sender is Hyperlink hyperlink && hyperlink.Name == "dashboard_hyperlink")
            {
                Dashboard dashboard = new();
             

                if (mainGrid.RowDefinitions.Count > 1)
                {
                    mainGrid.Children.Clear();
                }
                Grid.SetRow(navBarAdmin, 0);
                mainGrid.Children.Add(navBarAdmin);
                Grid.SetRow(dashboard, 1);
                mainGrid.Children.Add(dashboard);
            }
            else if(sender is Hyperlink hyperlink1 && hyperlink1.Name == "manage_hyperlink")
            {
                FinanceApp.Scenes.AdminPages.Manage manage = new();

                if (mainGrid.RowDefinitions.Count > 1)
                {
                    mainGrid.Children.Clear();
                }
                Grid.SetRow(navBarAdmin, 0);
                mainGrid.Children.Add(navBarAdmin);
                Grid.SetRow(manage, 1);
                mainGrid.Children.Add(manage);

            }
            else if (sender is Hyperlink hyperlink3 && hyperlink3.Name == "logout_hyperlink")
            {
                PurchasePage purchasePage = new();

                if (mainGrid.RowDefinitions.Count > 1)
                {
                    mainGrid.Children.Clear();
                }
                Grid.SetRow(navBar, 0);
                mainGrid.Children.Add(navBar);
                Grid.SetRow(purchasePage, 1);
                mainGrid.Children.Add(purchasePage);
            }
            else 
            {

            }
        }

    }
}
