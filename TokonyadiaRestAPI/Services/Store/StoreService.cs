using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;
using TokonyadiaRestAPII.Repositories;

namespace TokonyadiaRestAPII.Services;

public class StoreService : IStoreService
{
    private readonly IRepository<Store> _storeRepository;
    private readonly IPersistence _persistence;

    public StoreService(IRepository<Store> storeRepository, IPersistence persistence)
    {
        _storeRepository = storeRepository;
        _persistence = persistence;
    }

    public async Task<Store> CreateNewStore(Store payload)
    {
        try
        {
            var result = await _persistence.ExecuteTransactionAsync(async () =>
            {
                var store = await _storeRepository.SaveAsync(payload);
                await _persistence.SaveChangesAsync();
                return store;
            });
            return result;
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    public async Task<Store> GetById(string id)
    {
        try
        {
            var store = await _storeRepository.Find(store => store.Id.Equals(Guid.Parse(id)));
            if (store is null) return store;
            return store;
        }
        catch (Exception e)
        {
            return new Store();
        }
    }

    public async Task<PageResponse<StoreResponse>> GetAll(string? name, int page = 1, int size = 5)
    {
        var stores = await _storeRepository.FindAll
        (p => name == null || name == "" || p.StoreName.ToLower().Contains(name),
            page,
            size,
            new[] { "ProductPrices.Product" });

        var storeResponse = stores.Select(store =>
        {
            var productPriceResponse = store.ProductPrices.Select(productPrice =>
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
            return new StoreResponse()
            {
                Id = store.Id.ToString(),
                StoreName = store.StoreName,
                PhoneNumber = store.PhoneNumber,
                Address = store.Address,
                SiupNumber = store.SiupNumber,
                ProductPrices = productPriceResponse
            };
        }).ToList();

        var totalPages = (int)Math.Ceiling(await _storeRepository.Count() / (double)size);

        PageResponse<StoreResponse> pageResponse = new()
        {
            Content = storeResponse,
            TotalPages = totalPages,
            TotalElement = storeResponse.Count()
        };
        return pageResponse;
    }

    public async Task<Store> Update(Store payload)
    {
        if (payload.Id == Guid.Empty)
        {
            throw new Exception("Store not found!");
        }

        var result = await _persistence.ExecuteTransactionAsync(async () =>
        {
            var product = _storeRepository.Update(payload);
            await _persistence.SaveChangesAsync();
            return product;
        });

        return result;
    }

    public async Task DeleteById(string id)
    {
        var store = await _storeRepository.FindById(Guid.Parse(id));

        if (store is null) throw new Exception("Store not found!");

        _storeRepository.Delete(store);
        await _persistence.SaveChangesAsync();
    }
}