using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using FinanceAPPServer.Models;
using FirebaseAdmin.Messaging;
using LiveCharts;
using LiveCharts.Wpf;

namespace FinanceApp.ChartModels
{
    public class DoughnutModel : INotifyPropertyChanged
    {
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


        public DoughnutModel()
        {
            SeriesCollection = new SeriesCollection();
            LoadChartDataAsync();
        }

        private async void LoadChartDataAsync()
        {
            try
            {
                var kpiData = await FinanceApp.State.Api.FetchKpis();
                if (kpiData != null)
                {
                    UpdateChart(kpiData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading chart data: {ex.Message}");
            }
        }


        private void UpdateChart(KPI kpiData)
        {
            double totalExpenses = ConvertToDouble(kpiData.TotalExpenses);
            foreach (var category in kpiData.ExpensesByCategory)
            {
                string categoryName = category.Key;
                double categoryValue = ConvertToDouble(category.Value);

                SeriesCollection.Add(new PieSeries
                {
                    Title = categoryName,
                    Values = new ChartValues<double> { categoryValue },
                    DataLabels = true
                });

                SeriesCollection.Add(new PieSeries
                {
                    Title = $"{categoryName} of Total",
                    Values = new ChartValues<double> { totalExpenses - categoryValue },
                    DataLabels = true
                });
            }
        }

        private double ConvertToDouble(string value)
        {
            if (value.StartsWith("$"))
            {
                value = value.Substring(1);
            }
            return double.TryParse(value, out double result) ? result : 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
