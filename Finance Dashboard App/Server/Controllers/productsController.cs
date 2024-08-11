using FinanceAPPServer.Firebase;
using FinanceAPPServer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinanceAPPServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class productsController : ControllerBase
    {
        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<List<PRODUCT>> Get()
        {
            List<PRODUCT> products;
            try
            {
                products = await FireStoreDB.FetchProductsData();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in products get: "+e);
                throw;
            }

            if (products == null)
            {
                return await Task.FromResult<List<PRODUCT>>(null);  // Return Task<KPI> with null
            }

            return products;
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<PRODUCT> Get(string id)
        {
            PRODUCT product;

            try
            {
                product = await FireStoreDB.FetchProductById(id);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in products get: " + e);
                throw;
            }

            if (product == null)
            {
                return await Task.FromResult<PRODUCT>(null);  // Return Task<KPI> with null
            }

            return product;


        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PRODUCT product)
        {
            try
            {
                await FireStoreDB.CreateProduct(product.Expense,product.Price, product.Transactions.FirstOrDefault());

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] string transactionId)
        {
            try
            {
                await FireStoreDB.UpdateProduction(id, transactionId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await FireStoreDB.DeleteProduct(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
