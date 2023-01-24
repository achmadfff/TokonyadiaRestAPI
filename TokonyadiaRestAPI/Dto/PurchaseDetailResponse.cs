namespace TokonyadiaRestAPII.Dto;

public class PurchaseDetailResponse
{
    public string Id { get; set; }
    public int Qty { get; set; }
    public ProductResponse Product { get; set; }
    
}