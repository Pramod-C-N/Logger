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
        private readonly MyAppDbContext _appDbContext;

        public ProductController(MyAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var products = _appDbContext.Products.ToList();
                if (products.Count == 0)
                {
                    return NotFound("Products not available.");
                }
                return Ok(products);

            }
            catch (Exception ex )
            {

                return BadRequest(ex.Message);
            }    
        }
        [HttpGet("{id}")]
       
        public IActionResult  Get(int id)
        {
            try
            {
                var product = _appDbContext.Products.Find(id);
                if (product == null)
                {
                    return NotFound($"Product details not fount with id {id}");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {

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
