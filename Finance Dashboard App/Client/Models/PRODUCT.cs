using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;


namespace FinanceAPPServer.Models
{
    [FirestoreData]
    public class PRODUCT
    {
        [FirestoreProperty]
        public string? Id { get; set; }
        [FirestoreProperty]
        public string Price { get; set; }
        [FirestoreProperty]
        public string Expense { get; set; }
        [FirestoreProperty]
        public List<string>? Transactions { get; set; }
    }
}
