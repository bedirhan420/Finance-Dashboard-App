using FinanceAPPServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FinanceApp.ChartModels
{
    public class ProductsColumnModel : INotifyPropertyChanged
    {
        private ObservableCollection<PRODUCT> _products;
        public ObservableCollection<PRODUCT> Products
        {
            get { return _products; }
            private set // Make setter private to control access
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public ProductsColumnModel()
        {
            Products = new ObservableCollection<PRODUCT>();
            LoadProducts();
        }

        private async void LoadProducts()
        {
            try
            {
                var productList = await ProcessProducts();
                Products.Clear();
                foreach (var product in productList)
                {
                    Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Loading Products", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task<List<PRODUCT>> ProcessProducts()
        {
            var apiResponse = await FinanceApp.State.Api.FetchProducts();
            var response = apiResponse.Select(pr =>
            {
                if (double.TryParse(pr.Price.Replace("$", ""), out double price) &&
                   double.TryParse(pr.Expense.Replace("$", ""), out double expense))
                {
                    return new PRODUCT { Id = pr.Id, Price = price.ToString(), Expense = expense.ToString()};
                }
                return null;
            })
            .Where(x => x != null)
            .ToList();

            return response;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
