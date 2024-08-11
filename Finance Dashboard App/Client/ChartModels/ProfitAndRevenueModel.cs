using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Definitions.Series;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class RevenueProfitData
{
    public string Name { get; set; }
    public decimal Revenue { get; set; }
    public decimal Profit { get; set; }
}

public class ProfitAndRevenueModel : INotifyPropertyChanged
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

    public ProfitAndRevenueModel()
    {
        SeriesCollection = new SeriesCollection();
        LoadChartDataAsync();
    }

    private async void LoadChartDataAsync()
    {
        var data = await ProcessKPIs();
        UpdateChart(data);
    }

    private void UpdateChart(List<RevenueProfitData> data)
    {
        var revenueSeries = new LineSeries
        {
            Title = "Revenue",
            Values = new ChartValues<decimal>(data.Select(x => x.Revenue))
        };

        var profitSeries = new LineSeries
        {
            Title = "Profit",
            Values = new ChartValues<decimal>(data.Select(x => x.Profit))
        };

        Labels = data.Select(x => x.Name).ToArray();

        SeriesCollection.Clear();
        SeriesCollection.Add(revenueSeries);
        SeriesCollection.Add(profitSeries);
    }

    public async Task<List<RevenueProfitData>> ProcessKPIs()
    {
        var kpi = await FinanceApp.State.Api.FetchKpis(); // Varsayılan FetchKpis bir async metod olduğunu varsayıyorum
        if (kpi == null || kpi.MonthlyData == null)
            return new List<RevenueProfitData>();

        var resultList = kpi.MonthlyData.Select(md =>
        {
            if (decimal.TryParse(md.Revenue.Replace("$", ""), out decimal revenue) &&
                decimal.TryParse(md.Expenses.Replace("$", ""), out decimal expenses))
            {
                return new RevenueProfitData
                {
                    Name = md.Month?.Substring(0, 3),
                    Revenue = revenue,
                    Profit = revenue - expenses
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
