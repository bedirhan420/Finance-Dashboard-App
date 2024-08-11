using FinanceAPPServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

public class Transaction
{
    public string Id { get; set; }
    public string Buyer { get; set; }
    public double Amount { get; set; }

    public int Count { get; set; } 
}

namespace FinanceApp.ChartModels
{
    public class TransactionsColumnModel : INotifyPropertyChanged
    {
        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transactions
        {
            get { return _transactions; }
            private set // Make setter private to control access
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transaction));
            }
        }
        public TransactionsColumnModel()
        {
            Transactions = new ObservableCollection<Transaction>();
            LoadTransactions();
        }

        private async void LoadTransactions()
        {
            try
            {
                var transactionList = await ProcessTransactions();
                Transactions.Clear();
                foreach (var transaction in transactionList)
                {
                    Transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Loading Products", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async Task<List<Transaction>> ProcessTransactions()
        {
            var apiResponse = await FinanceApp.State.Api.FetchTransactions();
            var response = apiResponse.Select(tr =>
            {
                if (double.TryParse(tr.Amount.Replace("$", ""), out double amount) &&
                   int.TryParse(tr.ProductionIds.Count().ToString(), out int count))
                {
                    return new Transaction { Id = tr.Id,Buyer=tr.Buyer ,Amount = amount,Count = count };
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
