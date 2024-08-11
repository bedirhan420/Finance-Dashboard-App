using FinanceApp.ChartModels;
using FinanceAPPServer.Models;
using Firebase.Auth;
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

namespace FinanceApp.Scenes.AdminPages
{
    /// <summary>
    /// Manage.xaml etkileşim mantığı
    /// </summary>
    public partial class Manage : UserControl
    {
        public Manage()
        {
            InitializeComponent();
        }

        private async void create_button_Click(object sender, RoutedEventArgs e)
        {
            await CreateNewProduct();
        }

        private async Task CreateNewProduct()
        {
            var price = product_expense_textbox.Text;
            var expense = product_expense_textbox.Text;


            if (string.IsNullOrEmpty(price) || string.IsNullOrEmpty(expense))
            {
                MessageBox.Show("Tüm alanları doldurduğunuzdan emin olun");
                return;
            }


            if (!price.StartsWith("$"))
            {
                price = "$" + price;
            }

            if (!expense.StartsWith("$"))
            {
                expense = "$" + expense;
            }


            try
            {
                var transactions = new List<string>();
                PRODUCT newProduct = new PRODUCT
                {
                    Price = price,
                    Expense = expense,
                    Transactions = transactions 
                };
                await FinanceApp.State.Api.CreateProduct(newProduct);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }




        }
    }
}
