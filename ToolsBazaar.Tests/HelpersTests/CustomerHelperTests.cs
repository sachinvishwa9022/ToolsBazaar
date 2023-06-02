using FluentAssertions;
using ToolsBazaar.Domain.OrderAggregate;
using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.ProductAggregate;
using ToolsBazaar.Web.Helpers;

namespace ToolsBazaar.Tests.HelpersTests
{
    public class CustomerHelperTests
    {
        [Fact]
        public void GivenOrdersAndDateRangeAndCount_WhenFiltered_ThenShouldReturnCountNumberOfCustomers()
        {

            DateTime startDate = new DateTime(year: 2015, month: 1, day: 1);
            DateTime endDate = new DateTime(year: 2022, month: 12, day: 31);
            int highestCustomerId = 4;
            int lowestCustomerId = 2;
            decimal lowPrice = 2.0m;
            decimal highPrice = 200.0m;
            int requestCount = 5;

            List<Order> SampleOrders = GetSampleOrders(startDate, endDate, lowPrice, lowestCustomerId, highPrice, highestCustomerId, requestCount);

            CustomerHelper helper = new CustomerHelper();
            var filteredCustomer = helper.FilterOrdersForTopSpendingCustomers(SampleOrders, startDate, endDate, requestCount);

            filteredCustomer.Should().NotBeEmpty().And.HaveCount(requestCount,"Should match the count");
            filteredCustomer.Should().Contain(x => x.Id == highestCustomerId, "Should have the highest price customer's Id");
            filteredCustomer.First().As<Customer>().Id.Should().Be(highestCustomerId);
            filteredCustomer.Last().As<Customer>().Id.Should().Be(lowestCustomerId);

        }

        [Fact]
        public void GivenOrdersNotInDateRange_WhenFiltered_ThenShouldReturnNone()
        {
            DateTime startDate = new DateTime(year: 2015, month: 1, day: 1);
            DateTime endDate = new DateTime(year: 2022, month: 12, day: 31);
            DateTime requestStartDate = new DateTime(year: 2012, month: 1, day: 1);
            DateTime requestEndDate = new DateTime(year: 2013, month: 12, day: 31);
            int highestCustomerId = 4;
            int lowestCustomerId = 2;
            decimal lowPrice = 2.0m;
            decimal highPrice = 200.0m;
            int requestCount = 5;

            List<Order> SampleOrders = GetSampleOrders(startDate, endDate, lowPrice, lowestCustomerId, highPrice, highestCustomerId, requestCount);

            CustomerHelper helper = new CustomerHelper();
            var filteredCustomer = helper.FilterOrdersForTopSpendingCustomers(SampleOrders, requestStartDate, requestEndDate, requestCount);

            filteredCustomer.Should().BeEmpty();

        }

        [Fact]
        public void GivenLessItemsInOrders_WhenFilteredforMoreCounts_ThenShouldReturnWhateverIsAvailableInList()
        {
            DateTime startDate = new DateTime(year: 2015, month: 1, day: 1);
            DateTime endDate = new DateTime(year: 2022, month: 12, day: 31);
            int highestCustomerId = 4;
            int lowestCustomerId = 2;
            decimal lowPrice = 2.0m;
            decimal highPrice = 200.0m;
            int orderCount = 5;
            int requestCount = 20;

            List<Order> SampleOrders = GetSampleOrders(startDate, endDate, lowPrice, lowestCustomerId, highPrice, highestCustomerId, orderCount);

            CustomerHelper helper = new CustomerHelper();
            var filteredCustomer = helper.FilterOrdersForTopSpendingCustomers(SampleOrders, startDate, endDate, requestCount);

            filteredCustomer.Should().NotBeEmpty().And.HaveCount(orderCount);

        }


        [Fact]
        public void GivenOrdersInDateRange_WhenFilteredWithException_ThenShouldReturnNone()
        {
            DateTime startDate = new DateTime(year: 2015, month: 1, day: 1);
            DateTime endDate = new DateTime(year: 2022, month: 12, day: 31);
            DateTime requestStartDate = new DateTime(year: 2012, month: 1, day: 1);
            DateTime requestEndDate = new DateTime(year: 2013, month: 12, day: 31);
            int highestCustomerId = 4;
            int lowestCustomerId = 2;
            decimal lowPrice = 2.0m;
            decimal highPrice = 200.0m;
            int requestCount = 5;

            List<Order> SampleOrders = GetSampleOrders(startDate, endDate, lowPrice, lowestCustomerId, highPrice, highestCustomerId, requestCount);

            CustomerHelper helper = new CustomerHelper();
            helper.Invoking(y => y.FilterOrdersForTopSpendingCustomers(null, requestStartDate, requestEndDate, requestCount))
                .Should().Throw<Exception>();
        }

        private List<Order> GetSampleOrders(DateTime startDate, DateTime endDate, decimal lowestPrice, int lowPriceCustomerId, decimal highestPrice, int highestPriceCustomerId, int count = 10)
        {
            List<Order> orderList = new List<Order>();

            string customerNamePrefix = "CustomerName";

            for (int i = 1; i <= count; i++)
            {
                var order = new Order()
                {
                    Id = i,
                    Customer = new Customer()
                    {
                        Id = i,
                        Name = customerNamePrefix + i
                    },
                    Date = i == lowPriceCustomerId ? startDate : i == highestPriceCustomerId ? endDate : startDate.AddDays(new Random().Next(1, 2)),
                    Items = new()
                    {

                        new OrderItem()
                        {
                            Product = new Product()
                            {
                                Id = i,
                                Price = i == lowPriceCustomerId ? lowestPrice :  i == highestPriceCustomerId ? highestPrice : 10.0m * new Random().Next(1,2)
                            },
                            Quantity = i == lowPriceCustomerId || i == highestPriceCustomerId ? 1 : new Random().Next(1,3)

                        }
                    }
                };

                orderList.Add(order);
            }

            return orderList;
        }


    }
}
