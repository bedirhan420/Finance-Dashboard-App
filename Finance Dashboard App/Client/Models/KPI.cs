using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

[FirestoreData]
public class DayData
{
    [FirestoreProperty]
    public string? date { get; set; }

    [FirestoreProperty]
    public string? revenue { get; set; }

    [FirestoreProperty]
    public string? expenses { get; set; }

}

public class MonthData
{
    public string? Month { get; set; }
    public string? Revenue { get; set; }
    public string? Expenses { get; set; }
    public string? OperationalExpenses { get; set; }
    public string? NonOperationalExpenses { get; set; }
}


namespace FinanceAPPServer.Models
{
    [FirestoreData]
    public class KPI
    {
        [FirestoreProperty]
        public string? TotalProfit { get; set; }
        [FirestoreProperty]
        public string? TotalRevenue { get; set; }
        [FirestoreProperty]
        public string? TotalExpenses { get; set; }
        [FirestoreProperty]
        public Dictionary<string, string>? ExpensesByCategory { get; set; }
        [FirestoreProperty]
        public List<MonthData>? MonthlyData { get; set; }
        [FirestoreProperty]
        public List<DayData>? DailyData { get; set; }
    }
}
