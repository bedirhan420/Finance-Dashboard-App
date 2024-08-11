using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class OperationalExpensesData
{
    public string Name { get; set; }
    public decimal OperationalExpenses { get; set; }
    public decimal NonOperationalExpenses { get; set; }
}

namespace FinanceApp.ChartModels
{
    public class OperationalExpensesModel : INotifyPropertyChanged
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

        public OperationalExpensesModel()
        {
            SeriesCollection = new SeriesCollection();
            LoadChartDataAsync();
            ValueFormatter = value => string.Format("${0:N0}", value);
        }


        private async void LoadChartDataAsync()
        {
            var data = await ProcessKPIs();
            UpdateChart(data);
        }

        private void UpdateChart(List<OperationalExpensesData> data)
        {
            var revenueSeries = new LineSeries
            {
                Title = "Operational Expenses",
                Values = new ChartValues<decimal>(data.Select(x => x.OperationalExpenses))
            };

            var profitSeries = new LineSeries
            {
                Title = "Non Operational Expenses",
                Values = new ChartValues<decimal>(data.Select(x => x.NonOperationalExpenses))
            };

            Labels = data.Select(x => x.Name).ToArray();

            SeriesCollection.Clear();
            SeriesCollection.Add(revenueSeries);
            SeriesCollection.Add(profitSeries);
        }

        public async Task<List<OperationalExpensesData>> ProcessKPIs()
        {
            var kpi = await FinanceApp.State.Api.FetchKpis(); 

            var resultList = kpi.MonthlyData.Select(md =>
            {
                if (decimal.TryParse(md.OperationalExpenses.Replace("$", ""), out decimal operationalExpenses) &&
                    decimal.TryParse(md.NonOperationalExpenses.Replace("$", ""), out decimal nonOperationalExpenses))
                {
                    return new OperationalExpensesData
                    {
                        Name = md.Month?.Substring(0, 3),
                        OperationalExpenses = operationalExpenses,
                        NonOperationalExpenses = nonOperationalExpenses
                    };
                }
                return null;
            })
            .Where(x => x != null)
            .ToList();

            return resultList;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
