using Microsoft.AspNetCore.Mvc; // [Route], [ApiController], ControllerBase
using Packt.Shared; // Customer
using Northwind.WebApi.Repositories; // ICustomerRepository

namespace Northwind.WebApi.Controllers;

// base address: api/customers
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRespository repo;

    // constructor injects repository registered in Startup
    public CustomersController(ICustomerRespository repo)
    {
        this.repo = repo;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
    public async Task<IEnumerable<Customer>> GetCustomers(string? country)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            return await repo.RetrieveAllAsync();
        }
        else
        {
            return (await repo.RetrieveAllAsync()).Where(customer => customer.Country == country);
        }
    }

    // GET: api/customers/[id]
    [HttpGet("{id}", Name = nameof(GetCustomer))]
    [ProducesResponseType(200, Type = typeof(Customer))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCustomer(string id)
    {
        Customer? c = await repo.RetrieveAsync(id);
        if (c == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(c);
        }
    }

    // POST: api/customers
    // BODY: Customer (JSON, XML)
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Customer))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Create([FromBody] Customer c)
    {
        if (c == null)
        {
            return BadRequest();
        }

        Customer? addedCustomer = await repo.CreateAsync(c);

        if (addedCustomer == null)
        {
            return BadRequest("Repository failed to create customer");
        }
        else
        {
            return CreatedAtRoute( // 201 Created
                routeName: nameof(GetCustomer),
                routeValues: new { id = addedCustomer.CustomerId.ToLower() },
                value: addedCustomer);
        }
    }

    // PUT: api/customers/{id}
    // BODY: Customer (JSON, XML)
    [HttpPut("{id}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Update(
        string id, [FromBody] Customer c)
    {
        id = id.ToUpper();
        c.CustomerId = c.CustomerId.ToUpper();

        if (c == null || c.CustomerId != id)
        {
            return BadRequest(); // 400 Bad request
        }

        Customer? existing = await repo.RetrieveAsync(id);
        if (existing == null)
        {
            return NotFound(); // 404 Not Found
        }

        await repo.UpdateAsync(id, c);  

        return new NoContentResult(); // 204 No Content
    }

    // DELETE: api/customers/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(string id)
    {
        if (id == "bad")
        {
            ProblemDetails problemDetails = new()
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://localhost:5001/customers/failed-to-delete",
                Title = $"Customer Id {id} found but failed to delete.",
                Detail = "More details like Company Name, Country, and so on.",
                Instance = HttpContext.Request.Path
            };
            return BadRequest(problemDetails);
        }
        Customer? existing = await repo.RetrieveAsync(id);

        if (existing == null)
        {
            return NotFound(); // 404 Not Found
        }

        bool? deleted = await repo.DeleteAsync(id);

        if (deleted.HasValue && deleted.Value)
        {
            return new NoContentResult(); // 204 NoContent
        }
        else
        {
            return BadRequest( // 400 Bad Request
            $"Customer {id} was found but failed to delete."
            );
        }
    }
}