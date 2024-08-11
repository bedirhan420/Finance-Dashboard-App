using FinanceApp.ChartModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FinanceApp.Scenes
{
    public partial class Dashboard : UserControl
    {
        public RevenueAndExpensesModel AreaSeriesModel { get; private set; }
        public ProfitAndRevenueModel LineSeriesModel { get; private set; }
        public RevenueMonthByMonthModel ColumnChartModel { get; private set; }
        public OperationalExpensesModel LineSeriesModel1 { get; private set; }
        public ProductExpensesModel ScatterSeriesModel { get; private set; }
        public ProductsColumnModel ProductsColumnModel { get; private set; }
        public DoughnutModel DoughnutModel { get; private set; }

        public TransactionsColumnModel TransactionsColumnModel { get; private set; }

        public Dashboard()
        {
            InitializeComponent();

            Loaded += async (s, e) =>
            {
                AreaSeriesModel = new RevenueAndExpensesModel();
                LineSeriesModel = new ProfitAndRevenueModel();
                ColumnChartModel = new RevenueMonthByMonthModel();
                LineSeriesModel1 = new OperationalExpensesModel();
                ScatterSeriesModel = new ProductExpensesModel();
                ProductsColumnModel = new ProductsColumnModel();
                TransactionsColumnModel = new TransactionsColumnModel();
                DoughnutModel = new DoughnutModel();
                await InitializeDataAsync();
                
            };
        }

        private async Task InitializeDataAsync()
        {
            await Task.WhenAll(
                AreaSeriesModel.LoadChartData(),
                ColumnChartModel.InitializeChartData()
            );
            DataContext = this;
        }

     
    }
}
