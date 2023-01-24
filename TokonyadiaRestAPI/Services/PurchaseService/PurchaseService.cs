using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;
using TokonyadiaRestAPII.Repositories;
using TokonyadiaRestAPII.Services.Purchase;

namespace TokonyadiaRestAPII.Services.PurchaseService;

public class PurchaseService : IPurchaseService
{
    private readonly IRepository<Entities.Purchase> _purchaseRepository;
    private readonly IRepository<ProductPrice> _productPriceRepository;
    private readonly IRepository<Customer> _customerRepository;
    private readonly IPersistence _persistence;

    public PurchaseService(IRepository<Entities.Purchase> purchaseRepository, IPersistence persistence, IRepository<ProductPrice> productPriceRepository, IRepository<Customer> customerRepository)
    {
        _purchaseRepository = purchaseRepository;
        _persistence = persistence;
        _productPriceRepository = productPriceRepository;
        _customerRepository = customerRepository;
    }

    public async Task<PurchaseResponse> CreatePurchase(Entities.Purchase payload)
    {
        var cust = await _customerRepository.Find(x => x.Id.Equals(payload.CustomerId));
        if (cust is null)
        {
            throw new Exception("Customer Not Found");
        }
        
        var purchase = await _persistence.ExecuteTransactionAsync(async () =>
        {
            foreach (var purchaseDetail in payload.PurchaseDetails)
            {
                var productPrice = await _productPriceRepository.Find(x => x.Id.Equals(purchaseDetail.ProductPriceId),new []{"Product"});
                if (productPrice != null)
                {
                    if (purchaseDetail.Qty <= productPrice.Stock)
                    {
                        productPrice.Stock -= purchaseDetail.Qty;
                        _productPriceRepository.Update(productPrice);
                    }
                    else
                    {
                        throw new Exception("There is no stock for that qty");
                    }
                }
                else
                {
                    throw new Exception("Product Price Not Found!");
                }
            }
            payload.TransDate = DateTime.Now;
            var result = await _purchaseRepository.SaveAsync(payload);
            await _persistence.SaveChangesAsync();
            return result;
        });

        var purchaseResponse = new PurchaseResponse
        {
            Id = purchase.Id.ToString(),
            CustomerId = purchase.CustomerId.ToString(),
            TransDate = purchase.TransDate,
            PurchaseDetails = purchase.PurchaseDetails.Select(detail =>
            {
                return new PurchaseDetailResponse
                {
                    Id = detail.Id.ToString(),
                    Qty = detail.Qty,
                    Product = new ProductResponse()
                    {
                        Id = detail.ProductPrice.Product.Id.ToString(),
                        ProductName = detail.ProductPrice.Product.ProductName,
                        Description = detail.ProductPrice.Product.Description,
                        ProductPrices = detail.ProductPrice.Product.ProductPrices.Select(price =>
                            new ProductPriceResponse
                            {
                                Id = price.Id.ToString(),
                                Price = price.Price,
                                Stock = price.Stock,
                                StoreId = price.StoreId.ToString()
                            }).ToList()
                    }
                };
            }).ToList()
        };
        
        return purchaseResponse;
    }

    public async Task<PageResponse<PurchaseResponse>> GetAll(string? name, int page = 1, int size = 5)
    {
        var purchases = await _purchaseRepository.FindAll
        (p => name == null || name == "" || p.Customer.CustomerName.ToLower().Contains(name),
            page,
            size,
            new[] { "Customer","PurchaseDetails.ProductPrice.Product" });
        
        
        var purchaseResponse = purchases.Select(purchase =>
        {
            var purchaseResponse = new PurchaseResponse
            {
                Id = purchase.Id.ToString(),
                CustomerId = purchase.CustomerId.ToString(),
                TransDate = purchase.TransDate,
                PurchaseDetails = purchase.PurchaseDetails.Select(detail =>
                {
                    return new PurchaseDetailResponse
                    {
                        Id = detail.Id.ToString(),
                        Qty = detail.Qty,
                        Product = new ProductResponse()
                        {
                            Id = detail.ProductPrice.Product.Id.ToString(),
                            ProductName = detail.ProductPrice.Product.ProductName,
                            Description = detail.ProductPrice.Product.Description,
                            ProductPrices = detail.ProductPrice.Product.ProductPrices.Select(price =>
                                new ProductPriceResponse
                                {
                                    Id = price.Id.ToString(),
                                    Price = price.Price,
                                    Stock = price.Stock,
                                    StoreId = price.StoreId.ToString()
                                }).ToList()
                        }
                    };
                }).ToList()
            };
            return purchaseResponse;
        }).ToList();
        
        var totalPages = (int)Math.Ceiling(await _purchaseRepository.Count() / (double)size);
        PageResponse<PurchaseResponse> pageResponse = new()
        {
            Content = purchaseResponse,
            TotalPages = totalPages,
            TotalElement = purchaseResponse.Count()
        };

        return pageResponse;
    }
}