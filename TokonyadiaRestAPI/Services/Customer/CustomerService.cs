using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;
using TokonyadiaRestAPII.Repositories;

namespace TokonyadiaRestAPII.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IPersistence _persistence;

    public CustomerService(IRepository<Customer> customerRepository, IPersistence persistence)
    {
        _customerRepository = customerRepository;
        _persistence = persistence;
    }

    public async Task<Customer> CreateNewCustomer(Customer payload)
    {
        try
        {
            var result = await _persistence.ExecuteTransactionAsync(async () =>
            {
                var customer = await _customerRepository.SaveAsync(payload);
                await _persistence.SaveChangesAsync();
                return customer;
            });
            return result;
        }
        catch (Exception)
        {
            throw new Exception();
        }
        
    }

    public async Task<Customer> GetById(string id)
    {
        try
        {
            var customer = await _customerRepository.Find(customer => customer.Id.Equals(Guid.Parse(id)));
            if (customer is null) return customer;
            return customer;
        }
        catch (Exception e)
        {
            return new Customer();
        }
    }

    public async Task<PageResponse<CustomerResponse>> GetAll(string? name, int page = 1, int size = 5)
    {
        try
        {
            var customers = await _customerRepository.FindAll
            (p => name == null || name == "" || p.CustomerName.ToLower().Contains(name),
                page,
                size);
            
            var customerResponse = customers.Select(customer =>
            {
                return new CustomerResponse
                {
                    Id = customer.Id.ToString(),
                    CustomerName = customer.CustomerName,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    Email = customer.Email
                };
            }).ToList();
            
            var totalPages = (int)Math.Ceiling(await _customerRepository.Count() / (double)size);

            PageResponse<CustomerResponse> pageResponse = new()
            {
                Content = customerResponse,
                TotalPages = totalPages,
                TotalElement = customerResponse.Count()
            };
            return pageResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Customer> Update(Customer payload)
    {
        if (payload.Id == Guid.Empty)
        {
            throw new Exception("Customer not found!");
        }

        var result = await _persistence.ExecuteTransactionAsync(async () =>
        {
            var customer = _customerRepository.Update(payload);
            await _persistence.SaveChangesAsync();
            return customer;
        });

        return result;
    }

    public async Task DeleteById(string id)
    {
        var customer = await _customerRepository.FindById(Guid.Parse(id));

        if (customer is null) throw new Exception("Customer not found!");

        _customerRepository.Delete(customer);
        await _persistence.SaveChangesAsync();
    }
}