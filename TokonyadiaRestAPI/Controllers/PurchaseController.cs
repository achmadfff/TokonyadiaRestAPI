using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokonyadiaRestAPII.Dto;
using TokonyadiaRestAPII.Entities;
using TokonyadiaRestAPII.Services.Purchase;

namespace TokonyadiaRestAPII.Controllers;

[ApiController]
[Route("api/purchases")]
public class PurchaseController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePurchase(Purchase payload)
    {
        var purchaseResponse = await _purchaseService.CreatePurchase(payload);
        
        CommonResponse<PurchaseResponse> response = new()
        {
            StatusCode = (int)HttpStatusCode.Created,
            Message = "Succesfully Create Purchase!",
            Data = purchaseResponse
        };
        return Created("api/stores", response);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetPurchase([FromQuery] string? name, [FromQuery] int page = 1,
        [FromQuery] int size = 5)
    {
        var purchases = await _purchaseService.GetAll(name, page, size);
        
        CommonResponse<PageResponse<PurchaseResponse>> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Get Data",
            Data = purchases
        };
        
        return Ok(response);
    }
}