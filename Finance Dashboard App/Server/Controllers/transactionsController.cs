using FinanceAPPServer.Firebase;
using FinanceAPPServer.Models;
using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceAPPServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class transactionsController : ControllerBase
    {
        // GET: api/<TransactionsController>
        [HttpGet]
        public async Task<List<TRANSACTION>> Get()
        {
            List<TRANSACTION> transactions;
            try
            {
                transactions = await FireStoreDB.FetchTRANSACTIONSData();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in transactions get: " + e);
                throw;
            }

            if (transactions == null)
            {
                return await Task.FromResult<List<TRANSACTION>>(null);  // Return Task<KPI> with null
            }

            return transactions;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TRANSACTION transaction)
        {
            try
            {
                await FireStoreDB.CreateTransaction(transaction.Amount, transaction.Buyer, transaction.ProductionIds.FirstOrDefault());

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // PUT api/<TransactionsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] string productionId)
        {
            try
            {
                await FireStoreDB.UpdateTransaction(id, productionId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
