using ToolsBazaar.Domain.CustomerAggregate;
using ToolsBazaar.Domain.OrderAggregate;

namespace ToolsBazaar.Web.Helpers
{
    public class CustomerHelper : ICustomerHelper
    {
        public IEnumerable<Customer> FilterOrdersForTopSpendingCustomers(IEnumerable<Order> orders, DateTime startDate, DateTime endDate, int count)
        {

            try
            {
                // Sample Linq statement 
                //var query = (from order in orders
                //             where order.Date >= startDate && order.Date <= endDate
                //             group order by order.Customer into customerOrder
                //             select new { customer = customerOrder.Key, totalSpent = customerOrder.Sum(o => o.Items.Sum(i => i.Price)) })
                //       .OrderByDescending(o => o.totalSpent).Take(count);

                var topCustomers = orders.Where(o => o.Date >= startDate && o.Date <= endDate) // filter the date range
                    .GroupBy(x => x.Customer.Id) //group by customers
                    .Select(g =>
                    new
                    {
                        customer = g.Select(x=> x.Customer).First(),
                        totalSpending = g.Sum(order => order.Items.Sum(i => i.Price)) //calculate the total spending
                    }).OrderByDescending(c => c.totalSpending) //order by total spending in desc order
                    .Take(count); //Take only what needed

                return topCustomers.Select(x => x.customer);
            }
            catch (Exception)
            {
                throw; //Throw the exception to be handled by consumer
            }
        }
    }
}
