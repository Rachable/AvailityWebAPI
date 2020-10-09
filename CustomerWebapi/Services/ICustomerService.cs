using System.Collections.Generic;
using Models;

namespace Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer Add(Customer newCustomer);
        Customer GetById(int id);
        Customer Update(int id);
        void Remove(int id);
    }
}