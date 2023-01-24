using Microsoft.AspNetCore.Mvc;

namespace TokonyadiaRestAPII.Controllers;

[ApiController] // 1. Menandakan Controller ini API
[Route("hello")] // Menamai route api/endpoint
public class HelloController : ControllerBase // 2. Extend ControllerBas
{
    [HttpGet]
    public string GetHello()
    {
        return "Hello World";
    }
    
    // Path Variable, biasanya untuk mencari data spesifik
    // /hello/{id}
    [HttpGet("{id}")]
    public string GetHelloWithId(string id) 
    {
        return $"Hello {id}";
    }

    [HttpGet("query-param")]
    public string GetHelloWithQueryParam([FromQuery] string name,[FromQuery]bool isActive)
    {
        return $"Hello {name}, Active: {isActive}";
    }

    [HttpGet("object")]
    public object GetHelloWithObject()
    {
        return new
        {
            id = Guid.NewGuid(),
            name = "Achmad Fikri Fadhilah",
            isActive = true
        };
    }

    [HttpGet("array")]
    public List<object> GetHelloWithObjectArray()
    {
        return new List<object>
        {
            new
            {
            id = Guid.NewGuid(),
            name = "Achmad Fikri Fadhilah",
            isActive = true
            },
            new
            {
                id = Guid.NewGuid(),
                name = "Annida Salma Kasbina",
                isActive = true
            },
            new
            {
                id = Guid.NewGuid(),
                name = "Adliya Tsaqib Nur Kamila",
                isActive = true
            }
        };
        
    }

    [HttpPost]
    public object PostString([FromBody] object name)
    {
        return name;
    }
}