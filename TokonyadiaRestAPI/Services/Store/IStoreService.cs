using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;

namespace TokonyadiaRestAPII.Services;

public interface IStoreService
{
    Task<Store> CreateNewStore(Store payload);
    Task<Store> GetById(string id);
    Task<PageResponse<StoreResponse>> GetAll(string? name, int page = 1, int size = 5);
    Task<Store> Update(Store payload);
    Task DeleteById(string id);
}