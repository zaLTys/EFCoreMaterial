using DataAccessLayer.Models;
using FluentMigrator;

namespace DataAccessLayer.ManualMigrations;

[Tags("Postgres")]
[Migration(202501010004)]
public class UsingCtx2 : Migration
{
    private readonly MyDbContext _ctx;

    public UsingCtx2(MyDbContext ctx)
    {
        _ctx = ctx;
    }

    public override void Up()
    {
        Console.WriteLine("202501010004_UsingCtx2");

        var categories = new List<Category>();
        for (int i = 0; i < 30; i++)
        {
            var categoryId = Guid.NewGuid();
            var cat = new Category
            {
                Name = $"Category{i}",
                Id = categoryId,
                Products = new List<Product>()
            };

            for (int j = 1; j <= 30; j++)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = $"Product{j} of Category{i}",
                    Description = $"Product{j} of Category{i} Description",
                    Price = 10.99m + j,
                };
                cat.Products.Add(product);
            }

            categories.Add(cat);
        }

        using (var ctx = _ctx)
        {
            ctx.AddRange(categories);
            ctx.SaveChanges();
        }
    }

    public override void Down()
    {
        throw new NotImplementedException();
    }
}