using Models;

namespace Services
{
    public interface ICustomerService
    {
        PagedList<Customer> GetAllCustomers(CustomerParameters customerParameters);
        int Add(Customer newCustomer);
        Customer GetById(int id);
        bool Update(Customer newCustomer);
        bool Remove(int id);
    }
}