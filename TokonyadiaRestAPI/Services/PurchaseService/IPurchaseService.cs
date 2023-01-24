using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;

namespace TokonyadiaRestAPII.Services.Purchase;

public interface IPurchaseService
{
    Task<PurchaseResponse> CreatePurchase(Entities.Purchase payload);
    Task<PageResponse<PurchaseResponse>> GetAll(string? name, int page = 1, int size = 5);
}