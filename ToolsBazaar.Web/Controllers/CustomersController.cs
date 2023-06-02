using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.OrderAggregate;
using ToolsBazaar.Web.Helpers;

namespace ToolsBazaar.Web.Controllers;

public record CustomerDto(string Name);
[Authorize]
[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CustomersController> _logger;
    private readonly ICustomerHelper _customerHelper;

    public CustomersController(ILogger<CustomersController> logger, ICustomerRepository customerRepository, IOrderRepository orderRepository, ICustomerHelper customerHelper)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        _customerHelper = customerHelper;
    }

    [HttpPut("{customerId:int}")]
    public IActionResult UpdateCustomerName(int customerId, [FromBody] CustomerDto dto)
    {
        _logger.LogInformation($"Updating customer #{customerId} name...");

        if(dto == null)
        {
            return BadRequest();
        }

        var customer = _customerRepository.GetCustomerById(customerId);
        if(customer == null)
        {
            _logger.LogWarning($"The customer with #{customerId} not found");
            return NotFound();
        }

        _customerRepository.UpdateCustomerName(customerId, dto.Name);
        return NoContent();
    }

    [HttpGet]
    [Route("top")]
    public IActionResult TopCustomers()
    {
        try
        {
            var orders = _orderRepository.GetAll();

            if (orders == null || !orders.Any())
            {
                return NoContent();
            }

            DateTime startDate = new DateTime(year: 2015, month: 1, day: 1);
            DateTime endDate = new DateTime(year: 2022, month: 12, day: 31);
            int count = 5;

            var filteredCustomers = _customerHelper.FilterOrdersForTopSpendingCustomers(orders, startDate, endDate, count);

            return Ok(filteredCustomers);
        }
        catch(Exception ex)
        {
            _logger.LogWarning($"Error occured while proessing Customers {ex.Message}");
            return StatusCode(500);
        }
    }

}