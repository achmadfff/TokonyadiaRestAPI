using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokonyadiaRestAPII.Entities;

[Table("customer", Schema = "dbo")]
public class Customer
{
    [Key, Column(name: "id")] public Guid Id { get; set; }
    
    [Required]
    [Column(name: "customer_name", TypeName = "NVarchar(100)")]
    public string CustomerName { get; set; }
    
    [Required]
    [Column(name: "phone_number", TypeName = "NVarchar(14)")]
    public string PhoneNumber { get; set; }
    
    [Required]
    [Column(name: "address", TypeName = "NVarchar(100)")]
    public string Address { get; set; }

    [Required]
    [Column(name: "email", TypeName = "NVarchar(100)")]
    public string Email { get; set; }
    
    [Column(name:"user_id")] public Guid UserId { get; set; }
    
    public virtual User? User { get; set; }
    public override string ToString()
    {
        return $"{nameof(Id)}: {Id}, {nameof(CustomerName)}: {CustomerName}, " +
               $"{nameof(PhoneNumber)}: {PhoneNumber}, {nameof(Address)}: {Address}, {nameof(Email)}: {Email}";
    }
}