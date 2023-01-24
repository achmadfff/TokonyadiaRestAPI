using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;

namespace TokonyadiaRestAPII.Services;

public interface ICustomerService
{
    Task<Customer> CreateNewCustomer(Customer payload);
    Task<Customer> GetById(string id);
    Task<PageResponse<CustomerResponse>> GetAll(string? name, int page = 1, int size = 5);
    Task<Customer> Update(Customer payload);
    Task DeleteById(string id);
}