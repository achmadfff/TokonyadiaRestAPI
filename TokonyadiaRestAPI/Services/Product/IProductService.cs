using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;

namespace TokonyadiaRestAPII.Services;

public interface IProductService
{
    Task<ProductResponse> CreateNewProduct(Product payload);
    Task<ProductResponse> GetById(string id);
    Task<PageResponse<ProductResponse>> GetAll(string? name, int page = 1, int size = 5);
    Task<ProductResponse> Update(Product payload);
    Task DeleteById(string id);
}