using System;
using System.Collections.Generic;
using Models;
using System.Linq;

namespace Services
{
    public class MockCustomerService : ICustomerService
    {
        private List<Customer> _customers;

        public MockCustomerService()
        {
            _customers = new List<Customer>()
            {
                new Customer {Id = 1, FirstName = "Bob", LastName = "Burger", DateBecameCustomer = new DateTime(2019, 1, 20)},
                new Customer {Id = 2, FirstName = "Tony", LastName = "Stark", DateBecameCustomer = new DateTime()},
                new Customer {Id = 3, FirstName = "Bat", LastName = "Man", DateBecameCustomer = new DateTime(2020, 3, 15)},
                new Customer {Id = 4, FirstName = "Chicken", LastName = "Little", DateBecameCustomer = new DateTime(2019, 5, 7)},
                new Customer {Id = 5, FirstName = "Mary", LastName = "Poppins", DateBecameCustomer = new DateTime(2018, 1, 1)}
            };
        }
        public PagedList<Customer> GetAllCustomers(CustomerParameters customerParameters)
        {
            return PagedList<Customer>.ToPagedList(_customers.AsQueryable().OrderBy(on => on.Id),
                                                   customerParameters.PageNumber,
                                                   customerParameters.PageSize);
        }
        public int Add(Customer newCustomer)
        {
            newCustomer.DateBecameCustomer = DateTime.Now;
            newCustomer.Id = _customers.Max(x => x.Id) + 1;
            _customers.Add(newCustomer);
            return newCustomer.Id;
        }
        public Customer GetById(int id)
        {
            return _customers.Where(c => c.Id == id)
                             .FirstOrDefault();
        }
        public bool Update(Customer newCustomer)
        {
            var existingCustomer = GetById(newCustomer.Id);

            if (existingCustomer != null)
            {
                existingCustomer.FirstName = newCustomer.FirstName;
                existingCustomer.LastName = newCustomer.LastName;
                existingCustomer.DateLastModified = DateTime.Now;
                return true;
            }

            return false;
        }
        public bool Remove(int id)
        {
            var existingCustomer = GetById(id);

            if (existingCustomer == null)
                return false;

            _customers.Remove(existingCustomer);
            return true;
        }
    }
}