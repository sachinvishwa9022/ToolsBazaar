using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToolsBazaar.Domain.ProductAggregate;

namespace ToolsBazaar.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{

    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }


    [HttpGet]
    [Route("most-expensive")]
    public IActionResult GetMostExpensive()
    {
        _logger.LogInformation($"starting GetMostExpensive");

        var products = _productRepository.GetAll();

        //Check if producst are available
        if (products == null || !products.Any())
        {
            _logger.LogInformation("No products available");
            return NoContent();
        }

        //sort by Highest price then by name
        var orderedProduct = products.OrderByDescending(x => x.Price).ThenBy(p => p.Name);
        return Ok(orderedProduct);
    }

}