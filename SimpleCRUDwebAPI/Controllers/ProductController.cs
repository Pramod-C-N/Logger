using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleCRUDwebAPI.DAL;
using SimpleCRUDwebAPI.Models;

namespace SimpleCRUDwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //private readonly ILogger<ProductController> logger;

        private  readonly ILog logger;

        private readonly MyAppDbContext _appDbContext;

        public ProductController(MyAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            logger = LogManager.GetLogger(typeof(ProductController));
        }

        private void LogMethodExecution(string methodName, DateTime startTime, DateTime endTime, string status)
        {
            var logMessage = $"Method: {methodName}, Start Time: {startTime}, End Time: {endTime}, Status: {status}";
            logger.Info(logMessage);

            // insert this log message into the database using log4net
            ThreadContext.Properties["MethodName"] = methodName;
            ThreadContext.Properties["StartTime"] = startTime;
            ThreadContext.Properties["EndTime"] = endTime;
            ThreadContext.Properties["Status"] = status;
            logger.Info("Log this message in the database");
        }






        [HttpGet]
        public IActionResult Get()
        {
            logger.Info("Get Method called");

            try
            {
                var products = _appDbContext.Products.ToList();

                if (products.Count == 0)
                {
                    logger.Warn("No products available.");
                    return NotFound("Products not available.");
                }
                logger.Info("Get Method successful.");
                return Ok(products);

                

            }
            catch (Exception ex )
            {
              
                logger.Error("Error in YourMethod", ex);
                return BadRequest(ex.Message);
            }    
        }
        [HttpGet("{id}")]
       
        public IActionResult  Get(int id)
        {
            try
            {
                var startTime = DateTime.Now;
                var product = _appDbContext.Products.Find(id);
                var endTime = DateTime.Now;
                if (product == null)
                {
                    LogMethodExecution(nameof(Get), startTime, endTime, $"Product details not found with id {id}");
                    return NotFound($"Product details not fount with id {id}");
                }
                LogMethodExecution(nameof(Get), startTime, endTime, "Success");
                return Ok(product);
                
            }
            catch (Exception ex)
            {
                LogMethodExecution(nameof(Get), DateTime.Now, DateTime.Now, $"Error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(Product model)
        {
            try
            {
                _appDbContext.Products.Add(model);
                _appDbContext.SaveChanges();
                return Ok("Product Created");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(Product model)
        {
            if(model == null || model.Id == 0)
            {
               if(model == null)
                {
                    return BadRequest("Model data is invalid.");
                }
               else if(model.Id == 0 )
                {
                    return BadRequest($"Product Id {model.Id} is invalid.");
                }            
            }
            try
            {
                var product = _appDbContext.Products.Find(model.Id);
                if (product == null)
                {
                    return NotFound($"Product not found with id {model.Id}");
                }
                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.Qty = model.Qty;
                _appDbContext.SaveChanges();
                return Ok("Product details updated");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _appDbContext.Products.Find(id);
                if (product == null)
                {
                    return NotFound($"Product not found with id {id}");
                }
                _appDbContext.Remove(product);
                _appDbContext.SaveChanges();
                return Ok("Product details deleted.");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
