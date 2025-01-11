namespace DataAccessLayer.Models;

public class Category :BaseEntity
{
    public string Name { get; set; }
    
    // Nav
    public ICollection<Product> Products { get; set; }
}