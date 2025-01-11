using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly MyDbContext _ctx;
    
    public CategoriesController(MyDbContext ctx)
    {
        _ctx = ctx;
    }
    
    [HttpGet("all")]
    public IEnumerable<Category> GetAllCategories()
    {       
        return _ctx.Categories.Include(x=>x.Products).ToArray();
    }
    
    [HttpPost]
    public IActionResult CreateCategory([FromBody] Category category)
    {
            //validate and stuff
        _ctx.Categories.Add(category);
        _ctx.SaveChanges();

        return CreatedAtAction(nameof(CreateCategory), new { id = category.Id }, category);
    }
}