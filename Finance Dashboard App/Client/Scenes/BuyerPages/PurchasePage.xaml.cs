using FinanceAPPServer.Models;
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
    public partial class PurchasePage : UserControl
    {
        public PurchasePage()
        {
            InitializeComponent();
            PopulateProductsComboBox();
        }

        public async void PopulateProductsComboBox()
        {
            // Ürün listesini API'dan çekme
            List<PRODUCT> products = await FinanceApp.State.Api.FetchProducts();

            // ComboBox'ı temizle
            product_id_combobox.Items.Clear();

            // Ürün ID'lerini ComboBox'a ekleme
            foreach (var product in products)
            {
                product_id_combobox.Items.Add(product.Id); 
            }
        }

        private async void buy_button_Click(object sender, RoutedEventArgs e)
        {
            await BuyProduct();
        }

        private async Task BuyProduct()
        {
            try
            {
                // Fetch data
                List<TRANSACTION> transactions = await FinanceApp.State.Api.FetchTransactions();
                List<PRODUCT> products = await FinanceApp.State.Api.FetchProducts();
                KPI kpi = await FinanceApp.State.Api.FetchKpis();

                // Get user inputs
                var productId = product_id_combobox.Text;
                var buyer = buyer_textbox.Text;

                if (string.IsNullOrEmpty(productId) || string.IsNullOrEmpty(buyer))
                {
                    MessageBox.Show("Tüm alanları doldurduğunuzdan emin olun");
                    return;
                }

                var productIds = new List<string>();

                var existingTransaction = transactions.FirstOrDefault(t => t.Buyer == buyer);

                var currentMonth = DateTime.Now.Month.ToString(); 

                var product = products.FirstOrDefault(p => p.Id == productId);
                var monthRevenue = kpi.MonthlyData.FirstOrDefault(m => m.Month == currentMonth);

                double? monthRevenueValue = null;
                if (monthRevenue != null)
                {
                    try
                    {
                        monthRevenueValue = double.Parse(monthRevenue.Revenue.Replace("$", ""));
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Error parsing month revenue format.");
                        MessageBox.Show("Ay geliri verisi geçersiz formatta.");
                    }
                }

                if (existingTransaction != null)
                {
                    // Update existing transaction
                    await FinanceApp.State.Api.UpdateTransaction(existingTransaction.Id, productId);
                }
                else
                {
                    // Create new transaction
                    productIds.Add(productId);
                    TRANSACTION newTransaction = new TRANSACTION
                    {
                        Amount = product.Price,
                        Buyer = buyer,
                        ProductionIds = productIds
                    };
                    await FinanceApp.State.Api.CreateTransaction(newTransaction);
                }

                await FinanceApp.State.Api.UpdateProduct(productId, existingTransaction?.Id);

                var productPriceValue = product.Price.Replace("$", "");

                if (monthRevenueValue.HasValue)
                {
                    var newRevenue = monthRevenueValue.Value + double.Parse(productPriceValue);
                    var newRevenueStr = "$" + newRevenue.ToString(); 
                    await FinanceApp.State.Api.UpdateMonthRevenue(currentMonth, newRevenueStr);
                }

                MessageBox.Show("Ürün alma işleminiz başarıyla gerçekleştirilmiştir");

                // Clear input fields
                product_id_combobox.Text = "";
                buyer_textbox.Text = "";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

    }
}
