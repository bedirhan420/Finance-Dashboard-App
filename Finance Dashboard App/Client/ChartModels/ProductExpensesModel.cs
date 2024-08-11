using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


public class ProductExpensesData
{
    public string Id { get; set; }
    public double Price { get; set; }
    public double Expense { get; set; }
}

namespace FinanceApp.ChartModels
{
    public class ProductExpensesModel : INotifyPropertyChanged
    {
        private ChartValues<ScatterPoint> _scatterValues;
        public ChartValues<ScatterPoint> ScatterValues
        {
            get => _scatterValues;
            set
            {
                _scatterValues = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        private string[] _labels;
        public string[] Labels
        {
            get => _labels;
            set
            {
                _labels = value;
                OnPropertyChanged();
            }
        }

        public Func<double, string> ValueFormatter { get; private set; }

        public ProductExpensesModel()
        {
            ScatterValues = new ChartValues<ScatterPoint>();
            SeriesCollection = new SeriesCollection();
            ValueFormatter = value => string.Format("${0:N0}", value);
            LoadChartDataAsync();
        }

        private async void LoadChartDataAsync()
        {
            var products = await ProcessPRODUCTs();
            UpdateChart(products);
        }

        private void UpdateChart(List<ProductExpensesData> data)
        {
            var scatterSeries = new ScatterSeries
            {
                Title = "Product Expenses",
                Values = new ChartValues<ScatterPoint>(
                    data.Select(p => new ScatterPoint(p.Price, p.Expense))
                ),
                MinPointShapeDiameter = 3, 
                MaxPointShapeDiameter = 5  
            };

            Labels = data.Select(x => x.Id).ToArray();
            SeriesCollection.Clear();
            SeriesCollection.Add(scatterSeries);
        }

        public async Task<List<ProductExpensesData>> ProcessPRODUCTs()
        {
            var apiResponse = await FinanceApp.State.Api.FetchProducts();
            var response = apiResponse.Select(pr =>
            {
                if (double.TryParse(pr.Price.Replace("$", ""), out double price) &&
                    double.TryParse(pr.Expense.Replace("$", ""), out double expense))
                {
                    return new ProductExpensesData
                    {
                        Id = pr.Id,
                        Price = price,
                        Expense = expense
                    };
                }
                return null;
            })
            .Where(x => x != null)
            .ToList();

            return response;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
