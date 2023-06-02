using ToolsBazaar.Domain.CustomerAggregate;

namespace ToolsBazaar.Persistence;

public class CustomerRepository : ICustomerRepository
{
    public IEnumerable<Customer> GetAll() => DataSet.AllCustomers;

    public Customer GetCustomerById(int customerId)
    {
        var customer = DataSet.AllCustomers.FirstOrDefault(c => c.Id == customerId);
        return customer;
    }

    public void UpdateCustomerName(int customerId, string name)
    {
        var customer = DataSet.AllCustomers.FirstOrDefault(c => c.Id == customerId);
        customer?.UpdateName(name);
    }
}