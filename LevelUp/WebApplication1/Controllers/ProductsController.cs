using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly MyDbContext _ctx;
    
    public ProductsController(MyDbContext ctx)
    {
        _ctx = ctx;
    }
    
    [HttpGet("all")]
    public IEnumerable<Product> GetAllProducts()
    {       
        return _ctx.Products.ToArray();
    }
    
    [HttpPost]
    public IActionResult CreateProduct([FromBody] Product product)
    {
        var category = _ctx.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
        if (category == null)
        {
            return NotFound("Category not found.");
        }

        product.Category = category;
        _ctx.Products.Add(product);
        _ctx.SaveChanges();

        return CreatedAtAction(nameof(GetAllProducts), new { id = product.Id }, product);
    }
}