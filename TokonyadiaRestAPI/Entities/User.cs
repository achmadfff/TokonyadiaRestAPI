using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TokonyadiaRestAPII.Entities;

[Table(("user"))]
public class User
{
    [Key, Column(name:"id")] public Guid Id { get; set; }
    
    [Required]
    [Column(name:"username", TypeName = "Nvarchar(50)")]
    public string UserName { get; set; }
    
    [Required]
    [Column(name:"password_hash")]
    public byte[] PasswordHash { get; set; }
    
    [Required]
    [Column(name:"password_salt")]
    public byte[] PasswordSalt { get; set; }
    
    public virtual Customer? Customer { get; set; }
}