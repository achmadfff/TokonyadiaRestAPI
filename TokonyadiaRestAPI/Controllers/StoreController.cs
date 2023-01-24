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
[Route("api/stores")]
public class StoreController : ControllerBase
{
    private readonly IStoreService _storeService;

    public StoreController(IStoreService storeService)
    {
        _storeService = storeService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllStore([FromQuery] string? name,[FromQuery] int page = 1,[FromQuery] int size = 5 )
    {
        var storeResponse = await _storeService.GetAll(name, page, size);
        
        CommonResponse<PageResponse<StoreResponse>> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Get Data",
            Data = storeResponse
        };
        return Ok(response);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoreById(string id)
    {
        var storeResponse = await _storeService.GetById(id);

        CommonResponse<Store> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Get Data By Id",
            Data = storeResponse
        };
        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateNewStore([FromBody] Store payload)
    {
        var storeResponse = await _storeService.CreateNewStore(payload);

        CommonResponse<Store> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Create Store!",
            Data = storeResponse
        };
        return Created("api/stores", response);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateStore([FromBody] Store payload)
    {
        var storeResponse = await _storeService.Update(payload);
        
        CommonResponse<Store> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Succesfully Update Store!",
            Data = storeResponse
        };
        return Ok(response);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStore(string id)
    {
        await _storeService.DeleteById(id);
        CommonResponse<string> response = new()
        {
            StatusCode = (int)HttpStatusCode.OK,
            Message = "Successfully Delete Store",
            Data = null
        };

        return Ok(response);
    }
}