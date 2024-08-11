using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.FireBase
{
    [FirestoreData]
    public  class AdminDto
    {
        [FirestoreProperty]
        public  string? Email { get; set; }

        [FirestoreProperty]
        public string? Password { get; set; }
    }
}
