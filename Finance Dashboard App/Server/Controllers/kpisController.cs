using FinanceAPPServer.Firebase;
using FinanceAPPServer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceAPPServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class kpisController : ControllerBase
    {

        [HttpGet]
        public async Task<KPI> Get()
        {
            KPI kpis;
            try
            {
                kpis = await FireStoreDB.FetchKPIData();
            }
            catch (Exception ex)
            {
                // Handle potential exceptions during data retrieval
                Console.WriteLine($"Error retrieving KPI data: {ex}");  // Log or handle error appropriately
                return null;  // Or return a Task<KPI> representing an error
            }

            if (kpis == null)
            {
                return await Task.FromResult<KPI>(null);  // Return Task<KPI> with null
            }

            return kpis;
        }


        [HttpPut("{month}")]
        public async Task<IActionResult> Put(string month, [FromBody] string revenue)
        {
            try
            {
                await FireStoreDB.UpdateKpiExpense(month, revenue);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
