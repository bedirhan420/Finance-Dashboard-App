using Firebase.Auth; 
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Api;
using Google.Apis.Auth.OAuth2;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FinanceApp.Scenes
{
    public partial class NavBar : UserControl
    {
        private FirebaseAuthClient _firebaseAuth; 

        public NavBar()
        {
            InitializeComponent();           
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Hyperlink hyperlink && hyperlink.Name == "admin_login_hyperlink")
            {
                AdminLogin adminLogin = new AdminLogin();

                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                Grid mainGrid = mainWindow.MainGrid;

                if (mainGrid.RowDefinitions.Count > 1)
                {
                    mainGrid.Children.RemoveAt(1);
                }

                Grid.SetRow(adminLogin, 1);
                mainGrid.Children.Add(adminLogin);
            }
        }
    }
}
