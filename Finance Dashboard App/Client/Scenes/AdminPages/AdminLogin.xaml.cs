using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input; // Needed for KeyEventArgs

namespace FinanceApp.Scenes
{
    public partial class AdminLogin : UserControl
    {
        public AdminLogin()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(Control_KeyDown); 
            this.Focusable = true; 
        }

        private async void login_button_Click(object sender, RoutedEventArgs e)
        {
            await LoginControl();
        }

        private async void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
            {
                await AutoLogin();
            }
            else if (e.Key == Key.Enter)
            {
                e.Handled = true;
                login_button_Click(this, new RoutedEventArgs());
            }
        }

        private async Task AutoLogin()
        {
            mail_textbox.Text = "dmin123@gmail.com";
            password_textbox.Text = "987654321";
        }

        private async Task LoginControl()
        {
            string email = mail_textbox.Text;
            string password = password_textbox.Text;
            var result = await FinanceAPPServer.Firebase.FireBaseAuth.LogIn(email, password);

            if (result == true)
            {
                // Navigate to dashboard
                Dashboard dashboard = new Dashboard();
                NavBarAdmin navBarAdmin = new NavBarAdmin();
                MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                Grid mainGrid = mainWindow?.MainGrid;

                if (mainGrid?.RowDefinitions.Count > 1)
                {
                    mainGrid.Children.Clear();
                }
                Grid.SetRow(navBarAdmin, 0);
                mainGrid.Children.Add(navBarAdmin);
                Grid.SetRow(dashboard, 1);
                mainGrid.Children.Add(dashboard);
            }
            else
            {
                MessageBox.Show("Başarısız Giriş");
                mail_textbox.Text = "";
                password_textbox.Text = "";
            }
        }
    }
}
