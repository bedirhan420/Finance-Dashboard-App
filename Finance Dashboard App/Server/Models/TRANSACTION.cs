using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceAPPServer.Models
{
    [FirestoreData]
    public class TRANSACTION
    {
        [FirestoreProperty]
        public string? Id { get; set; }
        [FirestoreProperty]
        public string Amount { get; set; }
        [FirestoreProperty]
        public string? Buyer { get; set; }
        [FirestoreProperty]
        public List<string>? ProductionIds { get; set; }
    }
}
