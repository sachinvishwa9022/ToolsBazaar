using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.OrderAggregate;
using ToolsBazaar.Web.Controllers;
using ToolsBazaar.Web.Helpers;

namespace ToolsBazaar.Tests.ControllerTests
{
    public class CustomerControllerTests 
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerHelper _customerHelper;


        public CustomerControllerTests()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _orderRepository = Substitute.For<IOrderRepository>();
            _logger = Substitute.For<ILogger<CustomersController>>();
            _customerHelper = Substitute.For<ICustomerHelper>();
        }

        [Fact]
        public void GivenTopRequested_WhenCustomersAvailable_ThenShouldReturnOkWithValue()
        {

            _orderRepository.GetAll().Returns(new List<Order>() { new Order() { Id = 1} });
            _customerHelper.FilterOrdersForTopSpendingCustomers(Arg.Any<List<Order>>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>()).Returns(new List<Customer>() { new Customer() { Id=1} });

            CustomersController customerController = new CustomersController(_logger, _customerRepository, _orderRepository, _customerHelper);
            var result = customerController.TopCustomers();

            _orderRepository.Received().GetAll();
            _customerHelper.Received().FilterOrdersForTopSpendingCustomers(Arg.Any<List<Order>>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Is<int>(x => x == 5));

            result.Should().BeOfType(typeof(OkObjectResult), "OK result not returned");
            result.Should().NotBeNull();
            ((OkObjectResult)result).Value.Should().BeOfType(typeof(List<Customer>));
            ((List<Customer>)(((OkObjectResult)result).Value)).Should().NotBeEmpty();

        }
        [Fact]
        public void GivenTopRequested_WhenFilteredCustomerNotAvailable_ThenShouldReturnOkWithNoValue()
        {
            _orderRepository.GetAll().Returns(new List<Order>() { new Order() { Id = 1 } });
            _customerHelper.FilterOrdersForTopSpendingCustomers(Arg.Any<List<Order>>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>()).Returns(new List<Customer>());

            CustomersController customerController = new CustomersController(_logger, _customerRepository, _orderRepository, _customerHelper);
            var result = customerController.TopCustomers();

            _orderRepository.Received().GetAll();
            _customerHelper.Received().FilterOrdersForTopSpendingCustomers(Arg.Any<List<Order>>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Is<int>(x => x == 5));

            result.Should().BeOfType(typeof(OkObjectResult), "Ok Result Returned");
            result.Should().NotBeNull();
            ((OkObjectResult)result).Value.Should().BeOfType(typeof(List<Customer>));
            ((List<Customer>)(((OkObjectResult)result).Value)).Should().BeEmpty();
        }

        [Fact]
        public void GivenTopRequested_WhenOrdersNotAvailable_ThenShouldReturnNoContent()
        {
            _orderRepository.GetAll().Returns(new List<Order>());
            _customerHelper.FilterOrdersForTopSpendingCustomers(Arg.Any<List<Order>>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>());

            CustomersController customerController = new CustomersController(_logger, _customerRepository, _orderRepository, _customerHelper);
            var result = customerController.TopCustomers();

            _orderRepository.Received().GetAll();
            _customerHelper.DidNotReceive().FilterOrdersForTopSpendingCustomers(Arg.Any<List<Order>>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Is<int>(x => x == 5));

            result.Should().BeOfType(typeof(NoContentResult), "No content result not returned");
            result.Should().NotBeNull();
        }
        [Fact]
        public void GivenTopRequested_WhenExceptionOccured_ThenShouldReturnErrorStatus()
        {
            _orderRepository.GetAll().Returns(new List<Order>() { new Order() { Id = 1 } });
            _customerHelper.FilterOrdersForTopSpendingCustomers(Arg.Any<List<Order>>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>()).Throws(new Exception());

            CustomersController customerController = new CustomersController(_logger, _customerRepository, _orderRepository, _customerHelper);

            var result = customerController.TopCustomers();

            _orderRepository.Received().GetAll();
            _customerHelper.Received().FilterOrdersForTopSpendingCustomers(Arg.Any<List<Order>>(), Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Is<int>(x => x == 5));

            result.Should().BeOfType(typeof(StatusCodeResult), "Status code result not returned");
            ((StatusCodeResult)result).StatusCode.Should().Be(500);

        }

    }
}
