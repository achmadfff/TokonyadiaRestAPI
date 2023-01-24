using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;
using TokonyadiaRestAPII.Repositories;
using TokonyadiaRestAPII.Services;

namespace TokonyadiaRestAPII.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllCustomer([FromQuery] string? name,[FromQuery] int page = 1,[FromQuery] int size = 5 )
    {
        var storeResponse = await _customerService.GetAll(name, page, size);
        
        CommonResponse<PageResponse<CustomerResponse>> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Get Data",
            Data = storeResponse
        };
        return Ok(response);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(string id)
    {
        var customerResponse = await _customerService.GetById(id);

        CommonResponse<Customer> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Get Data By Id",
            Data = customerResponse
        };
        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateNewCustomer([FromBody] Customer customer)
    {
        var customerResponse = await _customerService.CreateNewCustomer(customer);
        
        CommonResponse<Customer> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Create Customer!",
            Data = customerResponse
        };
        return Created("api/stores", response);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] Customer customer)
    {
        var customerResponse = await _customerService.Update(customer);
        
        CommonResponse<Customer> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Update Customer!",
            Data = customerResponse
        };
        return Ok(response);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        await _customerService.DeleteById(id);
        CommonResponse<string> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Delete Customer",
            Data = null
        };

        return Ok(response);
    }
}