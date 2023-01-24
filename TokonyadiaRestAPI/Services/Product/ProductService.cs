using Microsoft.EntityFrameworkCore;
using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;
using TokonyadiaRestAPII.Repositories;

namespace TokonyadiaRestAPII.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepository;
    private readonly IPersistence _persistence;

    public ProductService(IRepository<Product> productRepository, IPersistence persistence)
    {
        _productRepository = productRepository;
        _persistence = persistence;
    }

    public async Task<ProductResponse> CreateNewProduct(Product payload)
    {
        var product = await _productRepository.Find(
            product => product.ProductName.ToLower().Equals(payload.ProductName.ToLower()), new[] { "ProductPrices" });

        if (product is null)
        {
            var result = await _persistence.ExecuteTransactionAsync(async () =>
            {
                var product = await _productRepository.SaveAsync(payload);
                await _persistence.SaveChangesAsync();
                return product;
            });

            var productPriceResponses = result.ProductPrices.Select(productPrice => new ProductPriceResponse
            {
                Id = productPrice.Id.ToString(),
                Price = productPrice.Price,
                Stock = productPrice.Stock,
                StoreId = productPrice.StoreId.ToString()
            }).ToList();

            ProductResponse response = new ProductResponse
            {
                Id = result.Id.ToString(),
                ProductName = result.ProductName,
                Description = result.Description,
                ProductPrices = productPriceResponses
            };

            return response;
        }

        var productPricesRequest = payload.ProductPrices.ToList();
        ProductPrice productPrice = new ProductPrice
        {
            Price = productPricesRequest[0].Price,
            Stock = productPricesRequest[0].Stock,
            StoreId = productPricesRequest[0].StoreId
        };
        
        product.ProductPrices.Add(productPrice);
        await _persistence.SaveChangesAsync();
        
        ProductResponse productResponse = new ProductResponse
        {
            Id = product.Id.ToString(),
            ProductName = product.ProductName,
            Description = product.Description,
            ProductPrices = new List<ProductPriceResponse>()
            {
                new ProductPriceResponse
                {
                    Id = productPrice.Id.ToString(),
                    Price = productPrice.Price,
                    Stock = productPrice.Stock,
                    StoreId = productPrice.StoreId.ToString()
                }
            }
        };
        return productResponse;
    }

    public async Task<ProductResponse> GetById(string id)
    {
            var product = await _productRepository.Find(p => p.Id.Equals(Guid.Parse(id)), new[] { "ProductPrices" });

            if (product is null) throw new Exception("Product Not Found!");

            var productPriceResponses = product.ProductPrices.Select(productPrice => new ProductPriceResponse()
            {
                Id = productPrice.Id.ToString(),
                Price = productPrice.Price,
                Stock = productPrice.Stock,
                StoreId = productPrice.StoreId.ToString()
            }).ToList();
            
            ProductResponse response = new ProductResponse
            {
                Id = product.Id.ToString(),
                ProductName = product.ProductName,
                Description = product.Description,
                ProductPrices = productPriceResponses
            };

            return response;
    }

    public async Task<PageResponse<ProductResponse>> GetAll(string? name, int page = 1, int size = 5)
    {
        // Cara Pertama
        var products = await _productRepository.FindAll
        (p => name == null || name == "" || p.ProductName.ToLower().Contains(name),
            page,
            size,
            new[] { "ProductPrices" });

        // Cara Kedua
        var products2 = await _productRepository.FindAll(
            criteria: p => EF.Functions.Like(p.ProductName, $"%{name}%"),
            page: page,
            size: size,
            includes: new[] { "ProductPrices" });

        //Cara Pertama
        var productResponses1 = products.Select(product =>
        {
            var productPriceResponses = product.ProductPrices.Select(productPrice =>
            {
                ProductPriceResponse productPriceResponse = new()
                {
                    Id = productPrice.Id.ToString(),
                    Price = productPrice.Price,
                    Stock = productPrice.Stock,
                    StoreId = productPrice.StoreId.ToString()
                };
                return productPriceResponse;
            }).ToList();

            return new ProductResponse
            {
                Id = product.Id.ToString(),
                ProductName = product.ProductName,
                Description = product.Description,
                ProductPrices = productPriceResponses
            };
        }).ToList();

        // Cara Kedua
        var productResponses2 = new List<ProductResponse>();
        var productPriceResponses = new List<ProductPriceResponse>();
        foreach (var product in products)
        {
            foreach (var productPrice in product.ProductPrices)
            {
                ProductPriceResponse productPriceResponse = new()
                {
                    Id = productPrice.Id.ToString(),
                    Price = productPrice.Price,
                    Stock = productPrice.Stock,
                    StoreId = productPrice.StoreId.ToString()
                };
                productPriceResponses.Add(productPriceResponse);
            }

            ProductResponse productResponse = new()
            {
                Id = product.Id.ToString(),
                ProductName = product.ProductName,
                Description = product.Description,
                ProductPrices = productPriceResponses
            };

            productResponses2.Add(productResponse);
        }

        var totalPages = (int)Math.Ceiling(await _productRepository.Count() / (double)size);
        PageResponse<ProductResponse> pageResponse = new()
        {
            Content = productResponses1,
            TotalPages = totalPages,
            TotalElement = productResponses1.Count()
        };

        return pageResponse;
    }

    public Task<ProductResponse> Update(Product payload)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteById(string id)
    {
        var product = await _productRepository.FindById(Guid.Parse(id));

        if (product is null) throw new Exception("Product Not Found!");
        _productRepository.Delete(product);
        await _persistence.SaveChangesAsync();
    }
}