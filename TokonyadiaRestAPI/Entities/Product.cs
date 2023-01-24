using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokonyadiaRestAPII.Entities;

[Table("product")]
public class Product
{
    [Key, Column(name:"id")] public Guid Id { get; set; }
    
    [Column(name:"product_name", TypeName = "Nvarchar(50)")]
    public string ProductName { get; set; }
    
    [Column(name:"description", TypeName = "Nvarchar(100)")]
    public string Description { get; set; }
    
    public ICollection<ProductPrice> ProductPrices { get; set; }
}