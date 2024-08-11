#region IMPORTS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using FinanceApp.FireBase;
using FinanceAPPServer.Models;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Google.Type;
using Grpc.Auth;
using Newtonsoft.Json;
#endregion

namespace FinanceAPPServer.Firebase
{
    public class FireStoreDB
    {

        #region DB CONNECTION
        public static FirestoreDb GetCredantials()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"cloud_fire.json";
            Console.WriteLine(path);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            FirestoreDb db = FirestoreDb.Create("finance-app-df41d");
            Console.WriteLine("DB Bağlantısı sağladı");
            return db;
        }

        static readonly FirestoreDb db = GetCredantials();
        #endregion

        #region READ ALL (k,p,t)
        public static async Task<KPI?> FetchKPIData()
        {
            try
            {
                // Reference the "kpis" collection and document with ID "76W72JUxDoBWCU3IvopD"
                DocumentReference docRef = db.Collection("kpis").Document("76W72JUxDoBWCU3IvopD");


                // Get the document snapshot asynchronously
                DocumentSnapshot docSnap = await docRef.GetSnapshotAsync();

                if (docSnap.Exists)
                {
                    // Convert Firestore data to KPIData object
                    Dictionary<string, object> firestoreData = docSnap.ToDictionary();
                    KPI kpiData = new KPI
                    {
                        TotalProfit = (string)firestoreData["totalProfit"],
                        TotalRevenue = (string)firestoreData["totalRevenue"],
                        TotalExpenses = (string)firestoreData["totalExpenses"],
                        ExpensesByCategory = (firestoreData.ContainsKey("expensesByCategory") ? ((Dictionary<string, object>)firestoreData["expensesByCategory"]).ToDictionary(k => k.Key, v => v.Value.ToString()) : new Dictionary<string, string>()),
                        MonthlyData = (firestoreData.ContainsKey("monthlyData") ? ((List<object>)firestoreData["monthlyData"]).Select(x => JsonConvert.DeserializeObject<MonthData>(JsonConvert.SerializeObject(x))).ToList() : new List<MonthData>()),
                        DailyData = (firestoreData.ContainsKey("dailyData") ? ((List<object>)firestoreData["dailyData"]).Select(x => JsonConvert.DeserializeObject<DayData>(JsonConvert.SerializeObject(x))).ToList() : new List<DayData>())
                    };

                    Console.WriteLine("Firestore KPI data:", kpiData);
                    return kpiData;
                }
                else
                {
                    Console.WriteLine("No such document!");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching KPI data:");
                Console.WriteLine(ex); // veya ex.StackTrace
                return null;
            }

        }

        public static async Task<List<PRODUCT?>> FetchProductsData()
        {

            try
            {

                DocumentReference docRef = db.Collection("products").Document("products");
                DocumentSnapshot docSnap = await docRef.GetSnapshotAsync();

                if (docSnap.Exists)
                {
                    Dictionary<string, object> firestoreData = docSnap.ToDictionary();

                    if (firestoreData.TryGetValue("products", out object productsObject))
                    {
                        List<PRODUCT> firestoreProducts = new List<PRODUCT>();
                        if (productsObject is IEnumerable<object> products)
                        {
                            foreach (var productData in products)
                            {
                                if (productData is Dictionary<string, object> productDict)
                                {
                                    firestoreProducts.Add(new PRODUCT
                                    {
                                        Id = productDict.ContainsKey("Id") ? productDict["Id"]?.ToString() : null,
                                        Price = productDict.ContainsKey("Price") ? (string)productDict["Price"] : null,
                                        Expense = productDict.ContainsKey("Expense") ? (string)productDict["Expense"] : null,
                                        Transactions = productDict.ContainsKey("Transactions") ? (productDict["Transactions"] as List<object>)?.ConvertAll(x => x is null ? null : x.ToString()) : null
                                });
                                }
                            }
                        }

                        Console.WriteLine("Firestore PRODUCTS data:", firestoreProducts);
                        return firestoreProducts;
                    }
                }

                Console.WriteLine("No such document!");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in fetchproducts: "+e);
                return null;
            }
        }

        public static async Task<List<TRANSACTION?>> FetchTRANSACTIONSData()
        {

            try
            {
                DocumentReference docRef = db.Collection("transactions").Document("transactions");
                DocumentSnapshot docSnap = await docRef.GetSnapshotAsync();

                if (docSnap.Exists)
                {
                    Dictionary<string, object> firestoreData = docSnap.ToDictionary();

                    if (firestoreData.TryGetValue("transactions", out object transactionsObject))
                    {
                        List<TRANSACTION> firestoreTransactions = new List<TRANSACTION>();

                        if (transactionsObject is IEnumerable<object> transactions)
                        {
                            foreach (var transactionData in transactions)
                            {
                                if (transactionData is Dictionary<string, object> transactionDict)
                                {
                                    firestoreTransactions.Add(new TRANSACTION
                                    {
                                        Id = transactionDict.ContainsKey("Id") ? transactionDict["Id"]?.ToString() : null,
                                        Amount = transactionDict.ContainsKey("Amount") ? (string)transactionDict["Amount"] : null,
                                        Buyer = transactionDict.ContainsKey("Buyer") ? transactionDict["Buyer"]?.ToString() : null,
                                        ProductionIds = transactionDict.ContainsKey("ProductionIds") ? (transactionDict["ProductionIds"] as List<object>)?.ConvertAll(x => x.ToString()) : null,
                                    });
                                }
                            }
                        }
                        return firestoreTransactions;
                    }
                }

                Console.WriteLine("No such document!");
                return null;
            }
            catch (Exception e )
            {
                Console.WriteLine("Error in fetch transactions: " + e);
                return null;
            }
   
        }

        #endregion

        #region READ BY ID (p,t)
        public static async Task<PRODUCT> FetchProductById(string productId)
        {
            try
            {
                DocumentReference docRef = db.Collection("products").Document("products");
                DocumentSnapshot docSnapshot = await docRef.GetSnapshotAsync();

                if (docSnapshot.Exists)
                {
                    Dictionary<string, object> data = docSnapshot.ToDictionary();

                    if (data.TryGetValue("products", out object productsObject))
                    {
                        if (productsObject is IEnumerable<object> products)
                        {
                            foreach (var productData in products)
                            {
                                if (productData is Dictionary<string, object> productDict)
                                {
                                    string id = productDict.TryGetValue("Id", out object idObj) ? idObj.ToString() : null;

                                    if (id == productId)
                                    {
                                        PRODUCT product = new PRODUCT
                                        {
                                            Id = productId,
                                            Expense = productDict.TryGetValue("Expense", out object expenseObj) ? expenseObj.ToString() : null,
                                            Price = productDict.TryGetValue("Price", out object priceObj) ? priceObj.ToString() : null,
                                            Transactions = productDict.ContainsKey("Transactions") ? (productDict["Transactions"] as List<object>)?.ConvertAll(x => x is null ? null : x.ToString()) : null
                                        };
                                        return product;
                                    }
                                }
                            }
                            Console.WriteLine($"Product with ID {productId} does not exist.");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No products found in the collection.");
                    }
                }
                else
                {
                    Console.WriteLine($"Product collection does not exist.");
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
        #endregion

        #region CREAT (p,t)

        public static async Task AddDocument(string collectionName, string documentId, object data)
        {
            CollectionReference collection = db.Collection(collectionName);

            DocumentReference docRef = collection.Document(documentId);

            await docRef.SetAsync(data);
        }

        public static async Task CreateTransaction(string amount,string buyer ,string productId)
        {

            List <TRANSACTION> transactions = await FetchTRANSACTIONSData();

            Dictionary<string, List<TRANSACTION>> map = new Dictionary<string, List<TRANSACTION>>();

            List<string> productIds = new List<string> { productId };

            TRANSACTION transaction = new TRANSACTION {
                Id = GenerateUniqueId().ToString(),
                Amount = amount,
                Buyer = buyer,
                ProductionIds = productIds
            };

            if (transactions.Any(t=>t.Buyer == transaction.Buyer))
            {
                return;
            }
            transactions.Add(transaction);
            map.Add("transactions", transactions);
            await AddDocument("transactions", "transactions",map);

        }

        public static async Task CreateProduct(string expense, string price, string transactionId)
        {

            List<PRODUCT> products = await FetchProductsData();

            Dictionary<string, List<PRODUCT>> map = new Dictionary<string, List<PRODUCT>>();

            List<string> transactionIds = new List<string> { transactionId };

            PRODUCT product = new PRODUCT
            {
                Id = GenerateUniqueId().ToString(),
                Expense = expense,
                Price=price,
                Transactions = transactionIds
            };

            if (products.Any(t => t.Id == product.Id))
            {
                return;
            }
            products.Add(product);
            map.Add("products",products);
            await AddDocument("products", "products", map);

        }

        #endregion

        #region UPDATE (k,p,t)
        public static async Task UpdateTransaction(string id,string productId)
        {
            List<TRANSACTION> transactions = await FetchTRANSACTIONSData();

            List<TRANSACTION> buyerTransactions = transactions
                .Where(t => t.Id?.Trim() == id?.Trim())
                .ToList();

            if (buyerTransactions.Count == 0)
            {
                Console.WriteLine("id ye sahip transaction bulunamadi");
                return;
            }

            foreach (var transaction in buyerTransactions)
            {
                transaction.ProductionIds.Add(productId);
            }

            Dictionary<string, List<TRANSACTION>> map = new()
            {
                { "transactions", transactions }
            };

            await AddDocument("transactions", "transactions", map);
        }

        public static async Task UpdateProduction(string id, string transactionId)
        {
            List<PRODUCT> products = await FetchProductsData();

            List<PRODUCT> buyingProducts = products
                .Where(t => t.Id?.Trim() == id?.Trim())
                .ToList();

            if (buyingProducts.Count == 0)
            {
                Console.WriteLine("id ye sahip product bulunamadi");
                return;
            }

            foreach (var product in buyingProducts)
            {
                product.Transactions.Add(transactionId);
            }

            Dictionary<string, List<PRODUCT>> map = new()
            {
                { "products", products }
            };

            await AddDocument("products", "products", map);
        }

        public static async Task UpdateKpiExpense(string date,string expense)
        {

            try
            {
                DocumentReference docRef = db.Collection("kpis").Document("76W72JUxDoBWCU3IvopD");

                KPI kpi = await FetchKPIData();

                List<DayData> dayDatas = kpi.DailyData.Where(d => d.date == date).ToList();

                if (dayDatas.Count == 0)
                {
                    Console.WriteLine("date ye sahip data bulunamadi");
                    return;
                }

                foreach (var dayData in dayDatas)
                {
                    dayData.expenses = expense;
                }

                await docRef.UpdateAsync("dailyData", dayDatas);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in Update KPI Expense : {e}");
                throw;
            }


        }

        #endregion

        #region DELETE (p)
        public static async Task DeleteProduct(string id)
        {
            try
            {
                DocumentReference productsDocRef = db.Collection("products").Document("products");
                DocumentSnapshot productsDocSnapshot = await productsDocRef.GetSnapshotAsync();

                if (!productsDocSnapshot.Exists)
                {
                    Console.WriteLine("Products belgesi bulunamadı.");
                    return;
                }

                Dictionary<string, object> productsData = productsDocSnapshot.ToDictionary();

                if (!productsData.ContainsKey("products") || !(productsData["products"] is List<object>))
                {
                    Console.WriteLine("Products dizisi bulunamadı veya geçersiz formatta.");
                    return;
                }

                List<object> productsList = (List<object>)productsData["products"];

                // Silinecek ürünleri filtrele
                List<object> deletingProducts = productsList
                    .Where(product => product is Dictionary<string, object> &&
                                      ((Dictionary<string, object>)product).ContainsKey("Id") &&
                                      ((string)((Dictionary<string, object>)product)["Id"]) == id)
                    .ToList();

                if (deletingProducts.Count == 0)
                {
                    Console.WriteLine("ID'ye sahip ürün bulunamadı.");
                    return;
                }

                foreach (var product in deletingProducts)
                {
                    productsList.Remove(product);
                }

                await productsDocRef.SetAsync(productsData);

                Console.WriteLine("Ürünler başarıyla silindi.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ürünler silinirken bir hata oluştu: {e}");
                throw;
            }
        }
        #endregion

        #region HELPER FUNCS
        public static ulong GenerateUniqueId()
        {
            string hexGuid = Guid.NewGuid().ToString("N").Substring(0, 16);
            // Ensure that the string is not too long for ulong.Parse
            if (hexGuid.Length > 16)
            {
                hexGuid = hexGuid.Substring(0, 16);
            }

            ulong id = ulong.Parse(hexGuid, System.Globalization.NumberStyles.HexNumber);

            return id;
        }
        #endregion

        #region ADMIN
        public static async Task<AdminDto> GetAdminData()
        {
            try
            {
                DocumentReference docRef = db.Collection("admin").Document("admin");

                DocumentSnapshot docSnap = await docRef.GetSnapshotAsync();

                if (docSnap.Exists)
                {
                    Dictionary<string, object> firestoreData = docSnap.ToDictionary();
                    AdminDto adminData = new()
                    {
                        Email = (string)firestoreData["email"],
                        Password = (string)firestoreData["password"],
                    };

                    return adminData;
                }
                else
                {
                    MessageBox.Show("No such document!");
                    return null;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show($" Error in get admin data {e}");
                return null;

            }
        }

        #endregion

    }
}
