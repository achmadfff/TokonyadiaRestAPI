using TokonyadiaRestAPII.Entities;

namespace TokonyadiaRestAPII.Dto;

public class PurchaseResponse
{
    public string Id { get; set; }
    public DateTime TransDate { get; set; }
    public string? CustomerId { get; set; }
    public ICollection<PurchaseDetailResponse>? PurchaseDetails { get; set; }
}