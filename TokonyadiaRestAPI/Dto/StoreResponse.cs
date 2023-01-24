namespace TokonyadiaRestAPII.Dto;

public class StoreResponse
{
    public String Id { get; set; }

    public string StoreName { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Address { get; set; }
    
    public string SiupNumber { get; set; }
    
    public List<ProductPriceResponse> ProductPrices { get; set; }
}