using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokonyadiaRestAPII.Entities;

[Table("store", Schema = "dbo")]
public class Store
{
    [Key, Column(name: "id")] public Guid Id { get; set; }

    [Required]
    [Column(name:"store_name",TypeName = "NVarchar(100)")]
    public string StoreName { get; set; }
    
    [Required]
    [Column(name:"phone_number",TypeName = "NVarchar(14)")]
    public string PhoneNumber { get; set; }
    
    [Required]
    [Column(name:"address",TypeName = "NVarchar(100)")]
    public string Address { get; set; }
    
    [Required]
    [Column(name:"siup_number",TypeName = "NVarchar(100)")]
    public string SiupNumber { get; set; }
    
    public ICollection<ProductPrice>? ProductPrices { get; set; }
}