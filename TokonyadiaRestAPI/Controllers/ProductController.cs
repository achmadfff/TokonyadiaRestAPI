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
[Route(("api/products"))]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateNewProduct([FromBody] Product request)
    {
        var productResponse = await _productService.CreateNewProduct(request);

        CommonResponse<ProductResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Succesfully create new Product",
            Data = productResponse
        };

        return Created("api/products", response);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var productResponse = await _productService.GetById(id);
        
        CommonResponse<ProductResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Get Data By Id",
            Data = productResponse
        };
        return Ok(response);
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllProduct([FromQuery] string? name,[FromQuery] int page = 1,[FromQuery] int size = 5 )
    {
        var products = await _productService.GetAll(name, page, size);
        
        CommonResponse<PageResponse<ProductResponse>> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Get Data",
            Data = products
        };
        
        return Ok(response);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductById(string id)
    {
        await _productService.DeleteById(id);
        CommonResponse<string> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Delete Product",
            Data = null
        };

        return Ok(response);
    }
}