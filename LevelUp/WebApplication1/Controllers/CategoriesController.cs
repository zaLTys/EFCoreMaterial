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
    
    [HttpGet("allSimple")]
    public async Task<IEnumerable<Category>> GetAllCategories()
    {       
        return await _ctx.Categories.Include(x=>x.Products).ToArrayAsync();
    }
    
    [HttpGet("allAsSplitQuery")]
    public IEnumerable<Category> GetAllAsSplitQuery()
    {       
        return _ctx.Categories
            .Include(x=>x.Products)
            .AsSplitQuery()
            .ToArray();
    }
    
        
    [HttpGet("allAsNoTracking")]
    public IEnumerable<Category> GetAllAsNoTracking()
    {       
        return _ctx.Categories
            .Include(x=>x.Products)
            .AsNoTracking()
            .ToArray();
    }
    
    [HttpGet("allAsSplitAsNoTracking")]
    public IEnumerable<Category> GetAllAsSplitAsNoTracking()
    {       
        return _ctx.Categories
            .Include(x=>x.Products)
            .AsNoTracking()
            .AsSplitQuery()
            .ToArray();
    }
    
    [HttpGet("ienumerable/{id}")]
    public Category? GetByIdIEnumerable(Guid id)
    {       
        return _ctx.Categories.ToList().FirstOrDefault(x => x.Id == id);
    }
    
    [HttpGet("iqueryable/{id}")]
    public Category? GetByIdIQueryable(Guid id)
    {       
        return _ctx.Categories.Where(x => x.Id == id).FirstOrDefault();
    }
    
    [HttpGet("names")]
    public IEnumerable<object> GetNames()
    {
        return _ctx.Categories
                    .Select(x => new { x.Name })
                    .ToList();
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