using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{


    private readonly ILogger<CategoriesController> _logger;
    private readonly MyDbContext _ctx;

    public CategoriesController(ILogger<CategoriesController> logger, MyDbContext ctx)
    {
        _logger = logger;
        _ctx = ctx;
    }

    [HttpGet("GetSimple")]
    public IEnumerable<Category> GetCategories()
    {
        return _ctx.Categories;
    }
    
    [HttpGet("GetCategoriesWithProducts")]
    public IEnumerable<Category> GetCategoriesWithProducts()
    {
        var asdf = _ctx.Categories
            .Include(x => x.Products)
            .AsNoTracking()
            .AsSplitQuery();
        return asdf;
    }
    
    [HttpGet("GetProducts")]
    public IEnumerable<Product> GetProducts()
    {
        return _ctx.Products;
    }
}