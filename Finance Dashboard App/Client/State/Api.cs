using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using FinanceAPPServer.Models;
using System.Windows;

namespace FinanceApp.State
{
    class Api
    {

        #region FETCH
        public static async Task<KPI> FetchKpis()
        {
            using var client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri("https://localhost:7264/");

                HttpResponseMessage response = await client.GetAsync("kpis");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var kpi = JsonConvert.DeserializeObject<KPI>(responseData);

                    if (kpi == null){return null;}

                    return kpi;
                }
                else
                {
                    //MessageBox.Show("API isteği başarısız oldu: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                return null;
            }
        }
        public static async Task<List<PRODUCT>> FetchProducts()
        {
            using var client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri("https://localhost:7264/");

                HttpResponseMessage response = await client.GetAsync("products");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var products = JsonConvert.DeserializeObject<List<PRODUCT>>(responseData);

                    if (products == null) { return null; }

                    return products;
                }
                else
                {
                    //MessageBox.Show("API isteği başarısız oldu: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                return null;
            }
        }
        public static async Task<List<TRANSACTION>> FetchTransactions()
        {
            using var client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri("https://localhost:7264/");

                HttpResponseMessage response = await client.GetAsync("transactions");

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var transactions = JsonConvert.DeserializeObject<List<TRANSACTION>>(responseData);

                    if (transactions == null) { return null; }

                    return transactions;
                }
                else
                {
                    //MessageBox.Show("API isteği başarısız oldu: " + response.StatusCode);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                return null;
            }
        }
        #endregion

        #region CREATE
        public static async Task<bool> CreateTransaction(TRANSACTION transaction)
        {
            using var client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri("https://localhost:7264/");

                // Serialize the TRANSACTION object to JSON
                var json = JsonConvert.SerializeObject(transaction);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send a POST request to the specified URI
                HttpResponseMessage response = await client.PostAsync("transactions", content);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Return true if the transaction was created successfully
                }
                else
                {
                    //MessageBox.Show("API isteği başarısız oldu: " + response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                return false;
            }
        }
        public static async Task<bool> CreateProduct(PRODUCT product)
        {
            using var client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri("https://localhost:7264/");

                // Serialize the TRANSACTION object to JSON
                var json = JsonConvert.SerializeObject(product);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send a POST request to the specified URI
                HttpResponseMessage response = await client.PostAsync("products", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Ürün başarıyla oluşturuldu");
                    return true; 
                }
                else
                {
                    //MessageBox.Show("API isteği başarısız oldu: " + response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                return false;
            }
        }

        #endregion

        #region UPDATE
        public static async Task<bool> UpdateTransaction(string id, string newProductionId)
        {
            using var client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri("https://localhost:7264/");

                var json = JsonConvert.SerializeObject(newProductionId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string requestUri = $"transactions/{id}";

                HttpResponseMessage response = await client.PutAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    //MessageBox.Show("API isteği başarısız oldu: " + response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                return false;
            }
        }
        public static async Task<bool> UpdateProduct(string id, string newTransactionId)
        {
            using var client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri("https://localhost:7264/");

                var json = JsonConvert.SerializeObject(newTransactionId);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string requestUri = $"products/{id}";

                HttpResponseMessage response = await client.PutAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    //MessageBox.Show("API isteği başarısız oldu: " + response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                return false;
            }
        }
        public static async Task<bool> UpdateMonthRevenue(string month, string revenue)
        {
            using var client = new HttpClient();
            try
            {
                client.BaseAddress = new Uri("https://localhost:7264/");

                var json = JsonConvert.SerializeObject(revenue);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string requestUri = $"kpis/{month}";

                HttpResponseMessage response = await client.PutAsync(requestUri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("API isteği başarısız oldu: " + response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}
