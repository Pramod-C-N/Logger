using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCRUDwebAPI.DAL;
using SimpleCRUDwebAPI.Models;
using System.Data.Common;

namespace SimpleCRUDwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //private readonly ILogger<ProductController> logger;

        // Declare a private field for the log4net logger And  Initialize the logger for the ProductController.
        private readonly ILog logger = LogManager.GetLogger(typeof(ProductController));

        // Declare a private field for the application's database context.
        private readonly MyAppDbContext _appDbContext;

        
        public ProductController(MyAppDbContext appDbContext)
        {
            // Assign the provided database context to the private field.
            _appDbContext = appDbContext;
            
        }

        private void LogMethodExecution(string methodName, DateTime startTime, DateTime endTime, string status)
        {
            //var logMessage = $"Method: {methodName}, Start Time: {startTime}, End Time: {endTime}, Status: {status}";
            //logger.Info(logMessage);

            // insert this log message into the database using log4net
            GlobalContext.Properties["MethodName"] = methodName;
            GlobalContext.Properties["StarTime"] = startTime;
            GlobalContext.Properties["EndTime"] = endTime;
            GlobalContext.Properties["Status"] = status;
            logger.Info("Log this message in the database");
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var startTime = DateTime.Now;
                var products = _appDbContext.Products.ToList();
                var endTime = DateTime.Now;

                if (products.Count == 0)
                {
                    LogMethodExecution(nameof(Get), startTime, endTime, "No products available.");
                    return NotFound("Products not available.");
                }

                LogMethodExecution(nameof(Get), startTime, endTime, "Success");
                return Ok(products);

            }
            catch (DbException dbEx)
            {
                // Handle database-specific exceptions
                LogMethodExecution(nameof(Get), DateTime.Now, DateTime.Now, $"Database Error: {dbEx.Message}");
                return StatusCode(500, "A database error occurred.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                LogMethodExecution(nameof(Get), DateTime.Now, DateTime.Now, $"Error: {ex.Message}");
                return BadRequest("An error occurred.");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetID(int id)
        {
            try
            {
                var startTime = DateTime.Now;
                var product = _appDbContext.Products.Find(id);
                var endTime = DateTime.Now;

                if (product == null)
                {
                    LogMethodExecution(nameof(GetID), startTime, endTime, $"Product details not found with ID: {id}");
                    return NotFound($"Product details not found with ID: {id}");
                }

                LogMethodExecution(nameof(GetID), startTime, endTime, "Success");
                return Ok(product);
            }
            catch (DbException dbEx)
            {
                // Handle database-specific exceptions
                LogMethodExecution(nameof(GetID), DateTime.Now, DateTime.Now, $"Database Error: {dbEx.Message}");
                return StatusCode(500, "A database error occurred.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                LogMethodExecution(nameof(GetID), DateTime.Now, DateTime.Now, $"Error: {ex.Message}");
                return BadRequest("An error occurred.");
            }
        }


        [HttpPost]
        public IActionResult Post(Product model)
        {
            try
            {
                var startTime = DateTime.Now;
                _appDbContext.Products.Add(model);
                _appDbContext.SaveChanges();
                var endTime = DateTime.Now;
                LogMethodExecution(nameof(Post), startTime, endTime, "Success");
                return Ok("Product Created");

            }
            catch (DbUpdateException dbEx)
            {
                // Handle database-specific exceptions
                LogMethodExecution(nameof(Post), DateTime.Now, DateTime.Now, $"Database Error: {dbEx.Message}");
                return StatusCode(500, "A database error occurred.");
            }
            catch (Exception ex)
            {
                LogMethodExecution(nameof(Post), DateTime.Now, DateTime.Now, $"Error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(Product model)
        {
            if (model == null || model.Id == 0)
            {
                if (model == null)
                {
                    return BadRequest("Model data is invalid.");
                }
                else if (model.Id == 0)
                {
                    return BadRequest($"Product Id {model.Id} is invalid.");
                }
            }
            try
            {
                var startTime = DateTime.Now;
                var product = _appDbContext.Products.Find(model.Id);
                var endTime = DateTime.Now;

                if (product == null)
                {
                    LogMethodExecution(nameof(Put), startTime, endTime, $"Product not found with id {model.Id}");
                    return NotFound($"Product not found with id {model.Id}");
                }

                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.Qty = model.Qty;
                _appDbContext.SaveChanges();

                LogMethodExecution(nameof(Put), startTime, endTime, "Success");
                return Ok("Product details updated");
            }
            catch (DbException dbEx)
            {
                // Handle database-specific exceptions
                LogMethodExecution(nameof(Put), DateTime.Now, DateTime.Now, $"Database Error: {dbEx.Message}");
                return StatusCode(500, "A database error occurred.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                LogMethodExecution(nameof(Put), DateTime.Now, DateTime.Now, $"Error: {ex.Message}");
                return BadRequest("An error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var startTime = DateTime.Now;
                var product = _appDbContext.Products.Find(id);
                var endTime = DateTime.Now;

                if (product == null)
                {
                    LogMethodExecution(nameof(Delete), startTime, endTime, $"Product not found with id {id}");
                    return NotFound($"Product not found with id {id}");
                }

                _appDbContext.Remove(product);
                _appDbContext.SaveChanges();

                LogMethodExecution(nameof(Delete), startTime, endTime, "Success");
                return Ok("Product details deleted.");
            }
            catch (DbException dbEx)
            {
                // Handle database-specific exceptions
                LogMethodExecution(nameof(Delete), DateTime.Now, DateTime.Now, $"Database Error: {dbEx.Message}");
                return StatusCode(500, "A database error occurred.");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                LogMethodExecution(nameof(Delete), DateTime.Now, DateTime.Now, $"Error: {ex.Message}");
                return BadRequest("An error occurred.");
            }
        }
    }
}
