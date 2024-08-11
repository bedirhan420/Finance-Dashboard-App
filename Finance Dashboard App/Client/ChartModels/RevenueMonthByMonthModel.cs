using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;

public class RevenueByMonth
{
    public string Name { get; set; }
    public decimal Revenue { get; set; }
}

namespace FinanceApp.ChartModels
{
    public class RevenueMonthByMonthModel
    {
        public SeriesCollection ColumnCollection { get; set; }
        public List<string> Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public RevenueMonthByMonthModel()
        {
            ColumnCollection = new SeriesCollection();
            Labels = new List<string>();
            Formatter = value => value.ToString("C");
        }

        public async Task InitializeChartData()
        {
            var revenueData = await FetchAndProcessKPIs();
            var values = new ChartValues<decimal>();
            foreach (var data in revenueData)
            {
                values.Add(data.Revenue);
                Labels.Add(data.Name);
            }

            ColumnCollection.Add(new ColumnSeries
            {
                Title = "Monthly Revenue",
                Values = values
            });
        }

        private async Task<List<RevenueByMonth>> FetchAndProcessKPIs()
        {
            var kpi = await FinanceApp.State.Api.FetchKpis();
            if (kpi == null || kpi.MonthlyData == null)
                return new List<RevenueByMonth>();

            return kpi.MonthlyData.Select(md =>
            {
                if (decimal.TryParse(md.Revenue.Replace("$", ""), out decimal revenue))
                {
                    return new RevenueByMonth
                    {
                        Name = md.Month?.Substring(0, 3),
                        Revenue = revenue,
                    };
                }
                return null;
            })
            .Where(x => x != null)
            .ToList();
        }
    }
}
