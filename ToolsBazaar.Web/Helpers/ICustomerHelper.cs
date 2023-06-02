using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.OrderAggregate;

namespace ToolsBazaar.Web.Helpers
{
    public interface ICustomerHelper
    {
        /// <summary>
        /// Filter the orders within given date range (inclusive) and return the count number of top spending customers
        /// </summary>
        /// <param name="orders">orders</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="count"></param>
        /// <returns>List of filtered customers</returns>
       public IEnumerable<Customer> FilterOrdersForTopSpendingCustomers(IEnumerable<Order> orders, DateTime startDate, DateTime endDate, int count);
    }
}
