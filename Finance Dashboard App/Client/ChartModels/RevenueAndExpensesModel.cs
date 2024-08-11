using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

public class RevenueExpensesData
{
    public string Name { get; set; }
    public decimal Revenue { get; set; }
    public decimal Expenses { get; set; }
}

namespace FinanceApp.ChartModels
{
    public class RevenueAndExpensesModel : INotifyPropertyChanged
    {
        private SeriesCollection _areaSeries;
        private ObservableCollection<string> _xLabels;

        public SeriesCollection AreaSeries
        {
            get => _areaSeries;
            set
            {
                _areaSeries = value;
                OnPropertyChanged(nameof(AreaSeries));
            }
        }

        public ObservableCollection<string> XLabels
        {
            get => _xLabels;
            set
            {
                _xLabels = value;
                OnPropertyChanged(nameof(XLabels));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public async Task<List<RevenueExpensesData>> ProcessKPIs()
        {
            var kpi = await FinanceApp.State.Api.FetchKpis();
            if (kpi == null || kpi.MonthlyData == null)
                return new List<RevenueExpensesData>();

            return kpi.MonthlyData.Select(md =>
            {
                decimal revenue = 0, expenses = 0;
                decimal.TryParse(md.Revenue.Replace("$", ""), out revenue);
                decimal.TryParse(md.Expenses.Replace("$", ""), out expenses);

                return new RevenueExpensesData
                {
                    Name = md.Month?.Substring(0, 3),
                    Revenue = revenue,
                    Expenses = expenses
                };
            }).ToList();
        }
        public async Task LoadChartData()
        {
            var data = await ProcessKPIs();

            var revenueValues = new ChartValues<decimal>();
            var expensesValues = new ChartValues<decimal>();
            var labels = new ObservableCollection<string>();

            foreach (var item in data)
            {
                revenueValues.Add(item.Revenue);
                expensesValues.Add(item.Expenses);
                labels.Add(item.Name);
            }

            AreaSeries = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Revenue",
                Values = revenueValues,
                PointGeometry = null
            },
            new LineSeries
            {
                Title = "Expenses",
                Values = expensesValues,
                PointGeometry = null
            }
        };

            XLabels = labels;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
